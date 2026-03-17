using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPTEST.Models
{
    /// <summary>
    /// Модель: Пользователи (сотрудники химчистки)
    /// Таблица: Users
    /// </summary>
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "ФИО обязательно")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "ФИО должно быть от 5 до 100 символов")]
        [Display(Name = "ФИО")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Некорректный Email адрес")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(255)]
        public string? PasswordHash { get; set; }

        [Required(ErrorMessage = "Роль обязательна")]
        [Display(Name = "Роль")]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }

        [StringLength(20)]
        [Phone(ErrorMessage = "Некорректный номер телефона")]
        [Display(Name = "Телефон")]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        [Display(Name = "Адрес")]
        public string? Address { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Дата регистрации")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [Display(Name = "Последний вход")]
        public DateTime? LastLoginDate { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive { get; set; } = true;

        // Навигационные свойства
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<OrderHistory>? OrderHistories { get; set; }
    }
}