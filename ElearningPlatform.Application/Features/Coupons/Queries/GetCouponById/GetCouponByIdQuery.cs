using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Coupons.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Coupons.Queries.GetCouponById
{
    public class GetCouponByIdQuery : IRequest<Result<CouponDetailsDto>>
    {
        public int Id { get; set; }

        public GetCouponByIdQuery(int id)
        {
            Id = id;
        }
    }
}
