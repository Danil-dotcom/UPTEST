using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPTEST.Models
{
    /// <summary>
    /// Модель: Услуги химчистки
    /// Таблица: Services
    /// </summary>
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Название услуги обязательно")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Название должно быть от 3 до 100 символов")]
        [Display(Name = "Услуга")]
        public string ServiceName { get; set; }

        [StringLength(500)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Базовая цена обязательна")]
        [Column(TypeName = "decimal(10, 2)")]
        [Range(0, 100000, ErrorMessage = "Цена должна быть от 0 до 100000")]
        [Display(Name = "Базовая цена")]
        public decimal BasePrice { get; set; }

        [Range(1, 720, ErrorMessage = "Длительность должна быть от 1 до 720 часов")]
        [Display(Name = "Длительность (часы)")]
        public int? DurationHours { get; set; }

        [Display(Name = "Активна")]
        public bool IsActive { get; set; } = true;

        // Навигационное свойство
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}