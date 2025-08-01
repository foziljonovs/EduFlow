﻿using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Users.Student;

public class StudentForFilterDto
{
    public DateTime? StartedDate { get; set; }
    public DateTime? FinishedDate { get; set; }
    public long? CourseId { get; set; }
    public EnrollmentStatus? CourseStatus { get; set; }
}
