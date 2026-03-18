using System.ComponentModel.DataAnnotations;

namespace UPTEST.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Укажите ФИО")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "ФИО должно быть от 5 до 100 символов")]
    [Display(Name = "ФИО")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите email")]
    [EmailAddress(ErrorMessage = "Введите корректный email")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите пароль")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть не короче 6 символов")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Подтвердите пароль")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
    [Display(Name = "Подтверждение пароля")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Введите корректный номер телефона")]
    [Display(Name = "Телефон")]
    public string? PhoneNumber { get; set; }

    [StringLength(200, ErrorMessage = "Адрес не должен превышать 200 символов")]
    [Display(Name = "Адрес")]
    public string? Address { get; set; }
}
