using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Courses.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Queries.GetFeaturedCourses
{
    public class GetFeaturedCoursesQuery : IRequest<Result<List<CourseUserDtoo>>>
    {
    }
    public class CourseUserDtoo
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public double AverageRating { get; set; }

        public int TotalStudents { get; set; }

        public int TotalLessons { get; set; }

        public int TotalDurationInMinutes { get; set; }

        public string ThumbnailUrl { get; set; }

        public string CategoryName { get; set; }

        public string InstructorName { get; set; }

        public string Language { get; set; }

        public string Level { get; set; }

        public bool IsFeatured { get; set; }
    }
}
