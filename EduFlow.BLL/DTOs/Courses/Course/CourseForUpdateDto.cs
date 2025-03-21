﻿using EduFlow.Domain.Entities.Users;
using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Courses.Course;

public class CourseForUpdateDto
{
    public string Name { get; set; }
    public double Price { get; set; }
    public DateTime Term { get; set; }
    public Status Archived { get; set; }
    public long TeacherId { get; set; }
    public long CategoryId { get; set; }
    public List<long> StudentIds { get; set; }
}
