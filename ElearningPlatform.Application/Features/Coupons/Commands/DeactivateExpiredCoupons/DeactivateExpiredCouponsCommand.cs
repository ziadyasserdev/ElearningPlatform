using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Coupons.Commands.DeactivateExpiredCoupons
{
    public class DeactivateExpiredCouponsCommand : IRequest<Result<int>>
    {
       
    }
}
