using ElearningPlatform.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Contracts.Repositories
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        public List<string> GetUserRoles(string id);
    }
}
