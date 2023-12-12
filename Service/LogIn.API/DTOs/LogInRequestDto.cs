using Core;
using FluentValidation;

namespace LogIn.API.DTOs;

public class LogInRequestDtoValidator :  AbstractValidator<LogInRequestDto>
{
    public LogInRequestDtoValidator()
    {
        this.RuleFor(dto => dto.UserName).NotEmpty();
        this.RuleFor(dto => dto.Password).NotEmpty();
    }
}

public class LogInRequestDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    
    public static implicit operator UserCredentials(LogInRequestDto dto)
    {
        return UserCredentials.From(dto.UserName, dto.Password);
    }
}

