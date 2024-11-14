using AmsAPI.Autorize.DTO;
using AmsAPI.Autorize.Enums;
using AmsAPI.Autorize.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SharedLibrary.OperationResult;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace AmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAccountService accountService, IOptions<AuthSettings> authOptions) : ControllerBase
    {
        private CookieOptions _CookieOptions => new()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.Add(authOptions.Value.Expires)
        };
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody]UserDataRequest user)
        {
            OperationResult result =  await accountService.Register(user);
            if (!result.IsSuccess)
                return BadRequest(result.Error.ErrorCode);
            return NoContent();
        }

        [HttpPost("LogOut")]
        [Authorize(nameof(Enum_Role.User))]
        public async Task<ActionResult> Logout()
        {
            string login = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            OperationResult result = await accountService.Logout(login);
                
            
            HttpContext.Response.Cookies.Delete(authOptions.Value.CookieName, _CookieOptions);
            return NoContent();
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody]UserDataRequest user)
        {
            //Возвращает jwt токен
            OperationResult<string> result = await accountService.Login(user, HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty);
            if (!result.IsSuccess)
                return BadRequest(result.Error.ErrorCode);
            HttpContext.Response.Cookies.Append(authOptions.Value.CookieName, result.Value, _CookieOptions);
            return NoContent();
        }

        [HttpGet("Data")]
        [Authorize(Roles = $"{nameof(Enum_Role.User)},{nameof(Enum_Role.Admin)}")]
        public ActionResult GetData()
        {
            var data = Enumerable.Range(1,10).Select(i => i*2).ToArray();
            return Ok(data);
        }

        [HttpPut("SetRoles")]
        [Authorize(Roles = nameof(Enum_Role.Admin))]
        public async Task<ActionResult> SetRolesForUser([FromBody]UserSetRolesCommand userWithRoles)
        {
            OperationResult result = await accountService.SetRoles(userWithRoles);
            if (!result.IsSuccess)
                return BadRequest(result.Error.ErrorCode);
            return NoContent();
        }

        [HttpPut("AddRole")]
        [Authorize(Roles = nameof(Enum_Role.Admin))]
        public async Task<ActionResult> AddRole([FromBody] UserAddRoleCommand userWithRole)
        {
            OperationResult result = await accountService.AddRole(userWithRole);
            if (!result.IsSuccess)
                return BadRequest(result.Error.ErrorCode);
            return NoContent();
        }

        [HttpDelete("RemoveRole")]
        [Authorize(Roles = nameof(Enum_Role.Admin))]
        public async Task<ActionResult> RemoveRole(RemoveRoleFromUserCommand userWithRemovedRole)
        {
            OperationResult result = await accountService.RemoveRole(userWithRemovedRole);
            if (!result.IsSuccess)
                return BadRequest(result.Error.ErrorCode);
            return NoContent();
        }
    }
}
