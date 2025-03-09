using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.DTOs.Messages.Message;
using EduFlow.BLL.Hubs;
using EduFlow.BLL.Interfaces.Messaging;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EduFlow.BLL.Services.Messaging;

public class MessageService(
    IUnitOfWork unitOfWork,
    NotificationHub notificationHub,
    IMapper mapper,
    ILogger<MessageService> logger) : IMessageService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly NotificationHub _notificationHub = notificationHub;
    private readonly ILogger<MessageService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    public async Task<IEnumerable<MessageForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var messages = await _unitOfWork.Message
                .GetAllAsync()
                .Where(x => x.IsDeleted == false)
                .ToListAsync();

            if (!messages.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Messages not found.");

            return _mapper.Map<IEnumerable<MessageForResultDto>>(messages);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all message. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<MessageForResultDto>> GetAllByCourseIdAsync(long courseId, CancellationToken cancellationToken = default)
    {
        try
        {
            var messages = await _unitOfWork.Message
                .GetAllAsync()
                .Where(x => x.CourseId == courseId && x.IsDeleted == false)
                .ToListAsync();

            if (!messages.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Messages not found.");

            return _mapper.Map<IEnumerable<MessageForResultDto>>(messages);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all message by course id: {courseId}. {ex}");
            throw;
        }
    }

    public async Task<IEnumerable<MessageForResultDto>> GetAllByStudentIdAsync(long studentId, CancellationToken cancellationToken = default)
    {
        try
        {
            var messages = await _unitOfWork.Message
                .GetAllAsync()
                .Where(x => x.StudentId == studentId && x.IsDeleted == false)
                .ToListAsync();

            if (!messages.Any())
                throw new StatusCodeException(HttpStatusCode.NotFound, "Messages not found.");

            return _mapper.Map<IEnumerable<MessageForResultDto>>(messages);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get all message by student id: {studentId}. {ex}");
            throw;
        }
    }

    public async Task<MessageForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var message = await _unitOfWork.Message.GetAsync(id);
            if (message is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Message not found.");

            if (message.IsDeleted)
                throw new StatusCodeException(HttpStatusCode.Gone, "This message has been deleted.");

            return _mapper.Map<MessageForResultDto>(message);
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while get message by id: {id}. {ex}");
            throw;
        }
    }

    public async Task<bool> SendAsync(MessageForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var existsCourse = await _unitOfWork.Course.GetAsync(dto.CourseId);
            if (existsCourse is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Course not found.");

            var existsStudent = await _unitOfWork.Student.GetAsync(dto.StudentId);
            if (existsStudent is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "Student not found.");

            if(string.IsNullOrEmpty(dto.Text))
                throw new StatusCodeException(HttpStatusCode.BadRequest, "Text is null or empty");

            var message = _mapper.Map<Message>(dto);
            var savedMessage = await _unitOfWork.Message.AddConfirmAsync(message);

            await _notificationHub.SendNotificationAsync(
                existsCourse.TeacherId.ToString(),
                dto.Text);

            _logger.LogInformation($"{existsCourse.TeacherId} - send message.");

            return savedMessage;
        }
        catch(Exception ex)
        {
            _logger.LogError($"An error occured while added the message. {ex}");
            throw;
        }
    }
}
