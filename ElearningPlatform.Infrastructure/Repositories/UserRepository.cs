using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Identity;
using ElearningPlatform.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public List<string> GetUserRoles(string id)
        {
            return (from ur in _dbContext.UserRoles
                    join r in _dbContext.Roles
                    on ur.RoleId equals r.Id
                    where ur.UserId == id
                    select r.Name).ToList();
        }
    }
}
