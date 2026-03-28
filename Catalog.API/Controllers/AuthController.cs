using Catalog.Application.Security;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtProvider _jwtProvider;

        public AuthController(IJwtProvider jwtProvider)
        {
            _jwtProvider = jwtProvider;
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // MİMARİ NOT: Normalde burada veritabanına (Users tablosuna) gidip 
            // şifrenin Hash'ini (Kriptolu halini) kontrol etmeliyiz. 
            // Şimdilik mimariyi test etmek için "Hardcoded" (Sabit) bir yönetici hesabı koyuyoruz.
            var adminId = "9d35ee33-7b0a-4f37-8e6a-2dc5ea42c6ac";
            if (request.Username == "admin" && request.Password == "123456")
            {
                var token = _jwtProvider.GenerateToken(adminId, request.Username, "Admin");

                return Ok(new { Token = token, Message = "Giriş başarılı! Bu Token'ı Swagger'a kopyalayın." });
            }

            // 2. Senaryo: Normal Müşteri Girişi (TEST İÇİN EKLİYORUZ)
            var customerId = "2f10aa44-8c1b-5g48-9f7b-3ed6fb53d7bd";
            if (request.Username == "asli" && request.Password == "123456")
            {
                var token = _jwtProvider.GenerateToken(customerId, request.Username, "User");
                return Ok(new { Token = token, Message = "Müşteri girişi başarılı!" });
            }

            return Unauthorized("Kullanıcı adı veya şifre hatalı!");
        }
    }
}
