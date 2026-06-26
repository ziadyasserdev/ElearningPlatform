using ElearningPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Questions.Dtos
{
    public class QuestionDetailsDto
    {
        public int Id { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public string QuestionType { get; set; }

        public decimal Score { get; set; }

        public int OrderIndex { get; set; }
    }
}
