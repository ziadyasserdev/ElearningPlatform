using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Commands.CreateCourse
{
    public class CreateCourseCommand:IRequest<Result<int>>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }

        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public DateTime? DiscountEndDate { get; set; }

        public int CategoryId { get; set; }
        public int InstructorId { get; set; }

        public string Language { get; set; }
        public int Level { get; set; }
    }
}
