using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.BLL.Interfaces.Payments;

namespace EduFlow.BLL.Services.Payments;

public class PaymentService : IPaymentService
{
    public Task<bool> AddToPayAsync(PaymentForCreateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PaymentForResultDto>> GetAllAsync(CancellationToken cancellation = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PaymentForResultDto>> GetAllByCourseIdAsync(long courseId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PaymentForResultDto>> GetAllByStudentIdAsync(long studentId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PaymentForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateToPayAsync(long id, PaymentForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
