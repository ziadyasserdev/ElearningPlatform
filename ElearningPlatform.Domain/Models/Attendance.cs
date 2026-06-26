using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Domain.Models
{
    public class Attendance : BaseEntity
    {
        public int LessonId { get; set; }

        public string StudentId { get; set; } = null!;

        public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;

        public DateTime CheckInTime { get; set; }

        public Lesson Lesson { get; set; } = null!;

        public ApplicationUser Student { get; set; } = null!;
        [NotMapped]
        public bool IsLate => Status == AttendanceStatus.Late;
    }
}
