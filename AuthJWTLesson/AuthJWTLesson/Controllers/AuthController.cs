using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthJWTLesson.DTOs;
using AuthJWTLesson.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthJWTLesson.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;

        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> Authenticate(AuthDTO authDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var token = await authService.Authenticate(authDTO.Login, authDTO.Password);
            if (String.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            return Ok(new { token });
            /*
             * + 1. Принимаем в параметрах объект с данными пользователя DTO (проверка на нулл знаечения)
             * + 2. Обращаемся к сервису который проводит аутентификацию
             * + 3. Получаем от сервиса токен
             *  + а) если токен пуст - кидаем 404 ошибка
             *  + б) если все ок - возравщаем токен в объекте
             */
        }
    }
}