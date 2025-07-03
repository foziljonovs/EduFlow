using EduFlow.BLL.Common.Pagination;
using EduFlow.BLL.DTOs.Payments.Payment;

namespace EduFlow.BLL.Interfaces.Payments;

public interface IPaymentService
{
    Task<PagedList<PaymentForResultDto>> GetAllAsync(int pageSize, int pageNumber, CancellationToken cancellation = default);
    Task<PaymentForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<long> AddToPayAsync(PaymentForCreateDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateToPayAsync(long id, PaymentForUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<PagedList<PaymentForResultDto>> GetAllByStudentIdAsync(int pageSize, int pageNumber, long studentId, CancellationToken cancellationToken = default);
    Task<PagedList<PaymentForResultDto>> GetAllByGroupIdAsync(int pageSize, int pageNumber, long groupId, CancellationToken cancellationToken = default);
    Task<PagedList<PaymentForResultDto>> FilterAsync(int pageSize, int pageNumber, PaymentForFilterDto dto, CancellationToken cancellation = default);
}
