﻿using EduFlow.Domain.Entities.Base;
using EduFlow.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduFlow.Domain.Entities.Courses;

public class Attendance : BaseEntity
{
    [Column("student_id")]
    public required long StudentId { get; set; }
    public Student Student { get; set; }
    [Column("group_id")]
    public required long GroupId { get; set; }
    public Group Group { get; set; }
    [Column("date")]
    public DateTime Date { get; set; }
    [Column("is_actived")]
    public bool IsActived { get; set; }
}
