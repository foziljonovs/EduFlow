using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.Desktop.Integrated.Helpers;

namespace EduFlow.Desktop.Integrated.Services.Payments.Payment;

public interface IPaymentService
{
    Task<List<PaymentForResultDto>> GetAllAsync();
    Task<PaymentForResultDto> GetByIdAsync(long id);
    Task<long> AddToPayAsync(PaymentForCreateDto dto);
    Task<bool> UpdateToPayAsync(long id, PaymentForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
    Task<List<PaymentForResultDto>> GetAllByStudentIdAsync(long studentId);
    Task<List<PaymentForResultDto>> GetAllByGroupIdAsync(long groupId);
    Task<PagedResponse<PaymentForResultDto>> FilterAsync(PaymentForFilterDto dto, int pageSize, int pageNumber);
    Task<PagedResponse<PaymentForResultDto>> GetAllPaginationAsync(int pageSize, int pageNumber);
    Task<List<PaymentForResultDto>> GetAllByTeacherIdAsync(long teacherId);
    Task<PagedResponse<PaymentForResultDto>> GetAllPaginationByTeacherIdAsync(long teacherId, int pageSize, int pageNumber);
}
