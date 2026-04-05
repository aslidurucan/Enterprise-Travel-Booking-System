using Catalog.Application.Security;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var emailInUse = await _userRepository.ExistsByEmailAsync(request.Email);
            if (emailInUse)
                throw new InvalidOperationException("Bu e-posta adresi zaten kayıtlı.");

            var usernameInUse = await _userRepository.ExistsByUsernameAsync(request.Username);
            if (usernameInUse)
                throw new InvalidOperationException("Bu kullanıcı adı zaten alınmış.");

            var passwordHash = _passwordHasher.Hash(request.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = "User",
                CreatedDate = DateTime.UtcNow
            };

            var created = await _userRepository.AddAsync(user);

            return new RegisterResult(created.Id, created.Username, created.Email, created.Role);
        }
    }
}
