﻿using EduFlow.Domain.Enums;

namespace EduFlow.Desktop.Integrated.Security;

public class IdentitySingelton
{
    public static IdentitySingelton identitySingelton;

    public static IdentitySingelton GetInstance()
    {
        if (identitySingelton == null)
            identitySingelton = new IdentitySingelton();

        return identitySingelton;
    }

    public string Token { get; set; } = string.Empty;
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    public void Reset()
    {
        Token = string.Empty;
        Id = 0;
        Name = string.Empty;
        PhoneNumber = string.Empty;
        Role = UserRole.Administrator;
    }
}
