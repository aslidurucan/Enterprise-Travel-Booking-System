using MediatR;

namespace Catalog.Application.Features.Auth.Commands.Login
{
    public record LoginCommand(string Username, string Password) : IRequest<LoginResult>; //validator dosyas??

    public record LoginResult(string Token, string Username, string Role);
}
