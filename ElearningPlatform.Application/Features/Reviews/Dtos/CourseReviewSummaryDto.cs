using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Dtos
{
    public class CourseReviewSummaryDto
    {
        public double AverageRating { get; set; }

        public int TotalReviews { get; set; }

        public int FiveStars { get; set; }

        public int FourStars { get; set; }

        public int ThreeStars { get; set; }

        public int TwoStars { get; set; }

        public int OneStar { get; set; }
    }
}
