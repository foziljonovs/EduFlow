using EduFlow.BLL.DTOs.Payments.Payment;
using EduFlow.Desktop.Integrated.Helpers;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Payments;
using EduFlow.Desktop.Integrated.Servers.Repositories.Payments;

namespace EduFlow.Desktop.Integrated.Services.Payments.Payment;

public class PaymentService : IPaymentService
{
    private readonly IPaymentServer _server;
    public PaymentService()
    {
        this._server = new PaymentServer();
    }
    public async Task<long> AddToPayAsync(PaymentForCreateDto dto)
    {
        try
        {
            var result = await _server.AddToPayAsync(dto);
            return result;
        }
        catch(Exception ex)
        {
            return -1;
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            var result = await _server.DeleteAsync(id);
            return result;
        }
        catch(Exception ex)
        {
            return false;
        }
    }

    public async Task<List<PaymentForResultDto>> FilterAsync(PaymentForFilterDto dto)
    {
        try
        {
            var result = await _server.FilterAsync(dto);
            return result;
        }
        catch(Exception ex)
        {
            return new List<PaymentForResultDto>();
        }
    }

    public async Task<List<PaymentForResultDto>> GetAllAsync()
    {
        try
        {
            var result = await _server.GetAllAsync();
            return result;
        }
        catch(Exception ex)
        {
            return new List<PaymentForResultDto>();
        }
    }

    public async Task<List<PaymentForResultDto>> GetAllByGroupIdAsync(long groupId)
    {
        try
        {
            var result = await _server.GetAllByGroupIdAsync(groupId);
            return result;
        }
        catch(Exception ex)
        {
            return new List<PaymentForResultDto>();
        }
    }

    public async Task<List<PaymentForResultDto>> GetAllByStudentIdAsync(long studentId)
    {
        try
        {
            var result = await _server.GetAllByStudentIdAsync(studentId);
            return result;
        }
        catch (Exception ex)
        {
            return new List<PaymentForResultDto>();
        }
    }

    public async Task<PagedResponse<PaymentForResultDto>> GetAllPaginationAsync(int pageSize, int pageNumber)
    {
        try
        {
            var result = await _server.GetAllPaginationAsync(pageSize, pageNumber);
            return result;
        }
        catch(Exception ex)
        {
            return new PagedResponse<PaymentForResultDto>();
        }
    }

    public async Task<PaymentForResultDto> GetByIdAsync(long id)
    {
        try
        {
            var result = await _server.GetByIdAsync(id);
            return result;
        }
        catch(Exception ex)
        {
            return new PaymentForResultDto();
        }
    }

    public async Task<bool> UpdateToPayAsync(long id, PaymentForUpdateDto dto)
    {
        try
        {
            var result = await _server.UpdateToPayAsync(id, dto);
            return result;
        }
        catch(Exception ex)
        {
            return false;
        }
    }
}
