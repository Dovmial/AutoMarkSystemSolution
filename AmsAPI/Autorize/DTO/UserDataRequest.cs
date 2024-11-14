using System.ComponentModel.DataAnnotations;

namespace AmsAPI.Autorize.DTO
{
    public record class UserDataRequest([Required] string Login, [Required] string Password);
}
