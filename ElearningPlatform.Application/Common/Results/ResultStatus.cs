using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Common.Results
{
    public enum ResultStatus
    {
        Success,
        ValidationError,
        NotFound,
        Conflict,
        Unauthorized,
        Forbidden,
        Failure,
        RequiresTwoFactor
    }
}
