using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Request
{
    public class UserRequestModel
    {
        [Required(ErrorMessage = "O e-mail é obrigatório")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatório")]
        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
