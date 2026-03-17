using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UPTEST.Models
{
    /// <summary>
    /// Модель: Роли пользователей
    /// Таблица: Roles
    /// </summary>
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Название роли обязательно")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Название роли должно быть от 2 до 50 символов")]
        [Display(Name = "Название роли")]
        public string? RoleName { get; set; }

        [StringLength(200, ErrorMessage = "Описание не может быть длиннее 200 символов")]
        [Display(Name = "Описание")]
        public string? Description { get; set; }

        // Навигационное свойство - связь с пользователями
        // ICollection<User> - User будет определен позже, это нормально для EF
        public virtual ICollection<User>? Users { get; set; }
    }
}