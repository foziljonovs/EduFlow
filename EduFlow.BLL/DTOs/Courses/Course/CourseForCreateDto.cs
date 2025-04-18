﻿using EduFlow.Domain.Enums;

namespace EduFlow.BLL.DTOs.Courses.Course;

public class CourseForCreateDto
{
    public string Name { get; set; }
    public double Price { get; set; }
    public byte Term { get; set; }
    public Status Archived { get; set; }
    public long CategoryId { get; set; }
}
