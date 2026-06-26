using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authentications.Dtos
{
    public class AuthResponseDto
    {
        public AuthTokenDto Auth { get; set; } = new();
        public UserResponseDto User { get; set; } = new();
    }
}
