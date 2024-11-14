using System.ComponentModel.DataAnnotations;

namespace AmsAPI.Autorize.DTO
{
    public record class UserAddRoleCommand([Required] string Login, [Required]string Role);
}
