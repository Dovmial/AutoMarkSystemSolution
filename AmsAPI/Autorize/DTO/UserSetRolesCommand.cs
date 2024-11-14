using System.ComponentModel.DataAnnotations;

namespace AmsAPI.Autorize.DTO
{
    public record class UserSetRolesCommand(
        [Required] string Login,
        params string[] roles);
}
