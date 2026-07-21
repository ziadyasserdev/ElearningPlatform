using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Orders.Queries.GetOrderDetails
{
    public class GetOrderDetailsValidator
        : AbstractValidator<GetOrderDetailsQuery>
    {
        public GetOrderDetailsValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0);
        }
    }
}
