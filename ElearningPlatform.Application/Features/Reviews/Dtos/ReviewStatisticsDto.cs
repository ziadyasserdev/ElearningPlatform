using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Dtos
{

    public class ReviewStatisticsDto
    {
        public int TotalReviews { get; set; }

        public int PendingReviews { get; set; }

        public int ApprovedReviews { get; set; }

        public int RejectedReviews { get; set; }

        public double AverageRating { get; set; }
    }
}
