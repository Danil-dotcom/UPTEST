using System.ComponentModel.DataAnnotations;

namespace UPTEST.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Укажите email")]
    [EmailAddress(ErrorMessage = "Введите корректный email")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Запомнить меня")]
    public bool RememberMe { get; set; }
}
