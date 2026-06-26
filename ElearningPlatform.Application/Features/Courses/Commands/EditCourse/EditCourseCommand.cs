using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Commands.EditCourse
{
    public class EditCourseCommand:IRequest<Result<int>>
    {
        public int CourseId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }

        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public DateTime? DiscountEndDate { get; set; }

        public string Language { get; set; }
        public int Level { get; set; }

        public int CategoryId { get; set; }
    }
}
