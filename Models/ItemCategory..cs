using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPTEST.Models
{
    /// <summary>
    /// Модель: Категории товаров
    /// Таблица: ItemCategories
    /// </summary>
    public class ItemCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Название категории обязательно")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Название должно быть от 3 до 50 символов")]
        [Display(Name = "Категория")]
        public string CategoryName { get; set; }

        [StringLength(200)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Column(TypeName = "decimal(3, 2)")]
        [Range(0.5, 3.0, ErrorMessage = "Коэффициент должен быть от 0.5 до 3.0")]
        [Display(Name = "Коэффициент цены")]
        public decimal BasePriceMultiplier { get; set; } = 1.0m;

        [Display(Name = "Требует особого ухода")]
        public bool RequiresSpecialCare { get; set; } = false;

        [Display(Name = "Активна")]
        public bool IsActive { get; set; } = true;

        // Навигационное свойство
        public virtual ICollection<Order> Orders { get; set; }
    }
}