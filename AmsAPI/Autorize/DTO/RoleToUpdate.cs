using System.ComponentModel.DataAnnotations;

namespace AmsAPI.Autorize.DTO
{
    public record class RoleToUpdate([Required]int Id, [Required] string Name);
}
