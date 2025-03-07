using AutoMapper;
using EduFlow.BLL.Common.Exceptions;
using EduFlow.BLL.Common.Security;
using EduFlow.BLL.DTOs.Users.User;
using EduFlow.BLL.Interfaces.Auth;
using EduFlow.BLL.Interfaces.Users;
using EduFlow.DAL.Interfaces;
using EduFlow.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace EduFlow.BLL.Services.Users;

public class UserService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ITokenService tokenService) : IUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ITokenService _tokenService = tokenService;
    public async Task<bool> ChangePasswordAsync(long id, UserForChangePasswordDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.User.GetAsync(id);
            if (user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "User not found.");

            if (!PasswordHelper.Verify(dto.CurrentPassword, user.Password, user.Salt))
                return false;

            var hasher = PasswordHelper.Hash(dto.NewPassword);
            user.Password = hasher.Hash;
            user.Salt = hasher.Salt;

            var result = await _unitOfWork.User.UpdateAsync(user);
            return result;
        }
        catch(Exception ex)
        {
            throw new Exception($"An error occured while change password the user. {id}");
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.User.GetAsync(id);
            if (user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "User not found.");

            user.IsDeleted = true;

            var result = await _unitOfWork.User.UpdateAsync(user);
            return true;
        }
        catch(Exception ex)
        {
            throw new Exception($"An error occured while delete the user. {id}");
        }
    }

    public async Task<IEnumerable<UserForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var users = _unitOfWork.User.GetAllAsync();
            if (!users.Any())
                throw new StatusCodeException(HttpStatusCode.NoContent, "Users not found.");

            return _mapper.Map<IEnumerable<UserForResultDto>>(users);
        }
        catch(Exception ex)
        {
            throw new Exception($"An error occured while get all the users.");
        }
    }

    public async Task<UserForResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.User.GetAsync(id);
            if (user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "User not found");

            return _mapper.Map<UserForResultDto>(user);
        }
        catch(Exception ex)
        {
            throw new Exception($"An error occured while get by id: {id} the user.");
        }
    }

    public async Task<string> LoginAsync(UserForLoginDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.User.GetAllAsync().FirstOrDefaultAsync(x => x.PhoneNumber == dto.PhoneNumber);
            if (user is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "User not found.");

            var hasher = PasswordHelper.Verify(dto.Password, user.Password, user.Salt);
            if (!hasher)
                throw new StatusCodeException(HttpStatusCode.BadRequest, "Password wrong!");

            return _tokenService.GenerateToken(user);
        }
        catch(Exception ex)
        {
            throw new Exception($"An error occured while login the user");
        }
    }

    public async Task<bool> RegisterAsync(UserForCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var userExists = await _unitOfWork.User.GetAllAsync().FirstOrDefaultAsync(x => x.PhoneNumber == dto.PhoneNumber);
            if (userExists is null)
                throw new StatusCodeException(HttpStatusCode.BadRequest, "User already exists!");

            var hasher = PasswordHelper.Hash(dto.Password);
            var savedUser = _mapper.Map<User>(dto);

            savedUser.Password = hasher.Hash;
            savedUser.Salt = hasher.Salt;
            
            return await _unitOfWork.User.AddConfirmAsync(savedUser);
        }
        catch(Exception ex)
        {
            throw new Exception($"An error occured while register the user");
        }
    }

    public async Task<bool> UpdateAsync(long id, UserForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var userExists = await _unitOfWork.User.GetAsync(id);
            if (userExists is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "User not found.");

            var updateUser = _mapper.Map<User>(dto);
            updateUser.Id = id;
            updateUser.UpdatedAt = DateTime.UtcNow.AddHours(5);

            return await _unitOfWork.User.UpdateAsync(userExists);
        }
        catch(Exception ex)
        {
            throw new Exception($"An error occured while update the user");
        }
    }

    public async Task<bool> VerifyPasswordAsync(long id, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var userExists = await _unitOfWork.User.GetAsync(id);
            if (userExists is null)
                throw new StatusCodeException(HttpStatusCode.NotFound, "User not found");

            if (!PasswordHelper.Verify(password, userExists.Password, userExists.Salt))
                return false;

            return true;
        }
        catch(Exception ex)
        {
            throw new Exception($"An error occured while verify password the user");
        }
    }
}
