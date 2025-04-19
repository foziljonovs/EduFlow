namespace EduFlow.Domain.Enums;

public enum EnrollmentStatus
{
    Pending = 0,       // Talaba hali kursga to‘liq qabul qilinmagan
    Active,            // Kursda faol ishtirok etyapti
    Suspended,         // Vaqtinchalik to‘xtatilgan (masalan: to‘lov qilmagan)
    Completed,         // Kursni muvaffaqiyatli tugatgan
    Dropped            // Kursdan chiqib ketgan yoki chetlatilgan
}
