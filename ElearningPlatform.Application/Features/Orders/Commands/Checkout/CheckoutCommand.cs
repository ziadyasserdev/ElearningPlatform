using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Orders.Commands.Checkout
{
    public class CheckoutCommand : IRequest<Result<int>>
    {
    }
}
