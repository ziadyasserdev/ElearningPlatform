using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string? IconUrl { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}
