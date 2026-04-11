using MediatR;

namespace Identity.Application.Features.Auth.Commands.Login
{
    public record LoginCommand(string Username, string Password) : IRequest<LoginResult>;

    public record LoginResult(string Token, string Username, string Role);
}
