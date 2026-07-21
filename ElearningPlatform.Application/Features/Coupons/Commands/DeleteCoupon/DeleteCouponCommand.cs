using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Coupons.Commands.DeleteCoupon
{
    public class DeleteCouponCommand : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public DeleteCouponCommand(int id)
        {
            Id = id;
        }
    }
}
