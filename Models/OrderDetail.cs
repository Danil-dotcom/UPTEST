using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPTEST.Models
{
    /// <summary>
    /// Модель: Детали заказа (связь с услугами)
    /// Таблица: OrderDetails
    /// </summary>
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }

        [Required(ErrorMessage = "ID заказа обязателен")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "ID услуги обязателен")]
        public int ServiceId { get; set; }

        [Range(1, 100, ErrorMessage = "Количество должно быть от 1 до 100")]
        public int Quantity { get; set; } = 1;

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        [Range(0, 100000, ErrorMessage = "Цена должна быть от 0 до 100000")]
        [Display(Name = "Цена за единицу")]
        public decimal PricePerUnit { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Сумма")]
        public decimal Subtotal { get; set; }

        [StringLength(200)]
        [Display(Name = "Примечания")]
        public string? Notes { get; set; }

        // Навигационные свойства
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service? Service { get; set; }
    }
}