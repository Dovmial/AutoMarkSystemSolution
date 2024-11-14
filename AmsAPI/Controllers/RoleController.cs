using AmsAPI.Autorize.DTO;
using AmsAPI.Autorize.Enums;
using AmsAPI.Autorize.Interfaces;
using AmsAPI.Autorize.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.OperationResult;

namespace AmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(
        IRoleRepository roleReposiory,
        ILogger<RoleController> logger) : ControllerBase
    {
        [HttpGet("GetRole")]
        [Authorize(Roles = nameof(Enum_Role.Admin))]
        public async Task<ActionResult<RoleResponse>> GetRole(string role)
        {
            OperationResult<Role> resultRole = await roleReposiory.Exists(role);
            if (!resultRole.IsSuccess)
            {
                logger.LogError(resultRole.Error.ErrorCode);
                return BadRequest(resultRole.Error.ErrorCode);
            }
            
            RoleResponse roleResponse = new RoleResponse(
                resultRole.Value.Id.Value,
                resultRole.Value.Name);
            return Ok(roleResponse);
        }

        [HttpGet("GetRoles")]
        [Authorize(Roles = nameof(Enum_Role.Admin))]
        public async Task<ActionResult<RoleResponse>> GetRoles()
        {
            OperationResult<ICollection<Role>> result = await roleReposiory.GetAll();
            if (!result.IsSuccess)
            {
                logger.LogError(result.Error.ErrorCode);
                return BadRequest(result.Error.ErrorCode);
            }
            IEnumerable<RoleResponse> response = result.Value.Select(x => new RoleResponse(x.Id.Value, x.Name));
            return Ok(response);
        }

        [HttpPost("AddRole")]
        [Authorize(Roles = nameof(Enum_Role.Admin))]
        public async Task<ActionResult> AddRole([FromBody]string role)
        {
            OperationResult<int> result = await roleReposiory.Add(new Role(role));
            if (!result.IsSuccess)
            {
                logger.LogError(result.Error.ErrorCode);
                return BadRequest(result.Error.ErrorCode);
            }
            return Ok(result.Value);
        }

        [HttpPut("UpdateRole")]
        [Authorize(Roles = nameof(Enum_Role.Admin))]
        public async Task<ActionResult> UpdateRole([FromBody]RoleToUpdate role)
        {
            OperationResult result = await roleReposiory.Update(new Role
            {
                Id = new RoleId(role.Id),
                Name = role.Name
            });
            if (!result.IsSuccess)
            {
                logger.LogError(result.Error.ErrorCode);
                BadRequest(result.Error.ErrorCode);
            }
            return NoContent();
        }

        [HttpDelete("DeleteRole")]
        [Authorize(Roles = nameof(Enum_Role.Admin))]
        public async Task<ActionResult> DeleteRole([FromQuery]string role)
        {
            OperationResult<Role> roleResult = await roleReposiory.Exists(role);
            if (!roleResult.IsSuccess)
            {
                logger.LogError(roleResult.Error.ErrorCode);
                return BadRequest(roleResult.Error.ErrorCode);
            }
            OperationResult result = await roleReposiory.Delete(roleResult.Value);
            if (!result.IsSuccess)
            {
                logger.LogError(result.Error.ErrorCode);
                return BadRequest(result.Error.ErrorCode);
            }
            logger.LogInformation("Role '{role}' is deleted", role);
            return NoContent();
        }
    }
}
