using ElearningPlatform.Application.Contracts.Payments;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Infrastructure.Payments
{

    public class StripeWebhookService : IStripeWebhookService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly StripeOptions stripeOptions;

        public StripeWebhookService(
            IUnitOfWork unitOfWork,
            IOptions<StripeOptions> stripeOptions)
        {
            this.unitOfWork = unitOfWork;
            this.stripeOptions = stripeOptions.Value;
        }

        public async Task HandleWebhookAsync(
            string json,
            string stripeSignature,
            CancellationToken cancellationToken = default)
        {
            Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    stripeSignature,
                    stripeOptions.WebhookSecret);
            }
            catch (Exception ex)
            {
                throw new Exception($"Invalid Stripe Webhook: {ex.Message}");
            }

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":

                    var succeededIntent = stripeEvent.Data.Object as PaymentIntent;

                    if (succeededIntent != null)
                    {
                        await HandlePaymentSucceededAsync(
                            succeededIntent,
                            cancellationToken);
                    }

                    break;

                case "payment_intent.payment_failed":

                    var failedIntent = stripeEvent.Data.Object as PaymentIntent;

                    if (failedIntent != null)
                    {
                        await HandlePaymentFailedAsync(
                            failedIntent,
                            cancellationToken);
                    }

                    break;
            }
        }

        private async Task HandlePaymentSucceededAsync(
            PaymentIntent paymentIntent,
            CancellationToken cancellationToken)
        {
            var payment = await unitOfWork.Payments.Query()
                .Include(x => x.Order)
                    .ThenInclude(x => x.OrderItems)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Coupon)
                .FirstOrDefaultAsync(x =>
                    x.PaymentIntentId == paymentIntent.Id,
                    cancellationToken);

            if (payment == null)
                return;

           
            if (payment.Status == PaymentStatus.Paid)
                return;

            payment.Status = PaymentStatus.Paid;
            payment.TransactionId = paymentIntent.Id;
            payment.PaidAt = DateTime.Now;

            payment.Order.Status = OrderStatus.Paid;
            payment.Order.PaidAt = DateTime.UtcNow;

            foreach (var item in payment.Order.OrderItems)
            {
                var exists = await unitOfWork.Enrollments.Query()
                    .AnyAsync(x =>
                        x.StudentId == payment.Order.StudentId &&
                        x.CourseId == item.CourseId,
                        cancellationToken);

                if (exists)
                    continue;

                var enrollment = new Enrollment
                {
                    StudentId = payment.Order.StudentId,
                    CourseId = item.CourseId,
                    OrderId = payment.Order.Id,
                    ProgressPercentage = 0,
                    Status = EnrollmentStatus.Active,
                    EnrolledAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                };

                await unitOfWork.Enrollments.AddAsync(enrollment);
            }

            if (payment.Order.CouponId.HasValue)
            {
                var coupon = await unitOfWork.Coupons.Query()
                    .FirstOrDefaultAsync(x =>
                        x.Id == payment.Order.CouponId.Value,
                        cancellationToken);

                if (coupon != null)
                {
                    coupon.UsedCount++;

                    var usage = new CouponUsage
                    {
                        CouponId = coupon.Id,
                        StudentId = payment.Order.StudentId,
                        OrderId = payment.Order.Id,
                        UsedAt = DateTime.Now,
                        CreatedAt = DateTime.Now
                    };

                    await unitOfWork.CouponUsages.AddAsync(usage);
                }
            }

            var cart = await unitOfWork.Carts.Query()
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(x =>
                    x.StudentId == payment.Order.StudentId,
                    cancellationToken);

            if (cart != null)
            {
                if (cart.CartItems.Any())
                {
                    unitOfWork.CartItems.DeleteRange(cart.CartItems);
                }

                cart.CouponId = null;
            }

            await unitOfWork.SaveAsync();
        }
                    private async Task HandlePaymentFailedAsync(
            PaymentIntent paymentIntent,
            CancellationToken cancellationToken)
        {
            var payment = await unitOfWork.Payments.Query()
                .Include(x => x.Order)
                .FirstOrDefaultAsync(x =>
                    x.PaymentIntentId == paymentIntent.Id,
                    cancellationToken);

            if (payment == null)
                return;

            if (payment.Status == PaymentStatus.Paid)
                return;

            payment.Status = PaymentStatus.Failed;
            payment.TransactionId = paymentIntent.Id;
            payment.FailureReason = paymentIntent.LastPaymentError?.Message;
            payment.UpdatedAt = DateTime.Now;

            payment.Order.Status = OrderStatus.Failed;
            payment.Order.UpdatedAt = DateTime.Now;

            await unitOfWork.SaveAsync();
        }
    }
}
  