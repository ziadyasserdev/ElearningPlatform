using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Carts.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Carts.Queries.GetMyCart
{
    public class GetMyCartQuery : IRequest<Result<CartResponse>>
    {
    }
}
