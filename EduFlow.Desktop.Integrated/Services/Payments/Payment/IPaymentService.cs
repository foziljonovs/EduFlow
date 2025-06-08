using EduFlow.BLL.DTOs.Payments.Payment;

namespace EduFlow.Desktop.Integrated.Services.Payments.Payment;

public interface IPaymentService
{
    Task<List<PaymentForResultDto>> GetAllAsync();
    Task<PaymentForResultDto> GetByIdAsync(long id);
    Task<bool> AddToPayAsync(PaymentForCreateDto dto);
    Task<bool> UpdateToPayAsync(long id, PaymentForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<List<PaymentForResultDto>> GetAllByStudentIdAsync(long studentId);
    Task<List<PaymentForResultDto>> GetAllByGroupIdAsync(long groupId);
}
