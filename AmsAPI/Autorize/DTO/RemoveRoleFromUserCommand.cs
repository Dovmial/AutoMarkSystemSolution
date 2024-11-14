using System.ComponentModel.DataAnnotations;

namespace AmsAPI.Autorize.DTO
{
    public record class RemoveRoleFromUserCommand([Required] string Login, [Required] string Role);
}
