namespace EduFlow.Domain.Enums;

public enum PaymentStatus
{
    Pending = 0,    // Kutilmoqda
    Completed,      // Yakunlangan
    Failed,         // Muvaffaqiyatsiz
    Refunded        // Qaytarilgan
}
