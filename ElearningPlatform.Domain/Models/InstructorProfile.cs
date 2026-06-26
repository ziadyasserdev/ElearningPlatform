using ElearningPlatform.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class InstructorProfile 
    {
        public int Id { get; set; }
        public string UserId { get; set; }                  
        public string Bio { get; set; } = string.Empty;      
        public string Specialization { get; set; }          
        public int ExperienceYears { get; set; }             
        public string LinkedInUrl { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public decimal Rating { get; set; }                 
        public int TotalStudents { get; set; }           
        public int TotalCourses { get; set; }               

        public ApplicationUser User { get; set; }
    }
}
