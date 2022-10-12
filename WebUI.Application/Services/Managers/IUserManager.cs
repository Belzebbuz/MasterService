﻿using Shared.Messages.Identity;
using Shared.Wrapper;

namespace WebUI.Application.Services.Managers;

public interface IUserManager : ITransientService
{
    Task<IResult<List<UserResponse>>> GetAllAsync();
    Task<IResult<IDR_003>> GetAsync(string userId);
    Task<IResult> RegisterUserAsync(IDM_001 registerRequest);
}
