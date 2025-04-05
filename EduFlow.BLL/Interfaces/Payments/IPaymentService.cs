    using EduFlow.BLL.DTOs.Payments.Payment;

namespace EduFlow.BLL.Interfaces.Payments;

public interface IPaymentService
{
    Task<IEnumerable<PaymentForResultDto>> GetAllAsync(CancellationToken cancellation = default);
    Task<PaymentForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> AddToPayAsync(PaymentForCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateToPayAsync(long id, PaymentForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentForResultDto>> GetAllByStudentIdAsync(long studentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentForResultDto>> GetAllByGroupIdAsync(long groupId, CancellationToken cancellationToken = default);
}
