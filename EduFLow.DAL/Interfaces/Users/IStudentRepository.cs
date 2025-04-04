﻿using EduFlow.Domain.Entities.Users;

namespace EduFlow.DAL.Interfaces.Users;

public interface IStudentRepository : IRepository<Student>
{
    IQueryable<Student> GetAllFullInformation();
}
