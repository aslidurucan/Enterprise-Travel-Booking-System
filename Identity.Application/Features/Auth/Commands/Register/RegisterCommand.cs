using MediatR;

namespace Identity.Application.Features.Auth.Commands.Register
{
    public record RegisterCommand(string Username, string Email, string Password) : IRequest<RegisterResult>;

    public record RegisterResult(Guid UserId, string Username, string Email, string Role);
}
