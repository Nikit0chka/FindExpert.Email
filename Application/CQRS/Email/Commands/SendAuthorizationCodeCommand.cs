using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Application.CQRS.Email.Commands;

public sealed record SendAuthorizationCodeCommand(string Email, string AuthorizationCode):ICommand<Result>;