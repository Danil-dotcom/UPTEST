using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPTEST.Models
{
    /// <summary>
    /// Модель: Заказы (основная таблица)
    /// Таблица: Orders
    /// </summary>
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Номер заказа обязателен")]
        [StringLength(20)]
        [Display(Name = "Номер заказа")]
        public string OrderNumber { get; set; }

        [Required(ErrorMessage = "Менеджер обязателен")]
        [Display(Name = "Менеджер")]
        public int UserId { get; set; }

        [Display(Name = "Постоянный клиент")]
        public int? CustomerId { get; set; }

        [Required(ErrorMessage = "Имя клиента обязательно")]
        [StringLength(100)]
        [Display(Name = "Имя клиента")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Телефон клиента обязателен")]
        [StringLength(20)]
        [Phone(ErrorMessage = "Некорректный телефон")]
        [Display(Name = "Телефон клиента")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "Категория обязательна")]
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }

        [StringLength(200)]
        [Display(Name = "Описание вещи")]
        public string ItemDescription { get; set; }

        [StringLength(100)]
        [Display(Name = "Тип пятна")]
        public string StainType { get; set; }

        [Range(1, 100, ErrorMessage = "Количество должно быть от 1 до 100")]
        [Display(Name = "Количество")]
        public int Quantity { get; set; } = 1;

        [Required(ErrorMessage = "Статус обязателен")]
        [StringLength(30)]
        [Display(Name = "Статус")]
        public string Status { get; set; } = "Принят";

        [StringLength(20)]
        [Display(Name = "Приоритет")]
        public string Priority { get; set; } = "Обычный";

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Общая сумма")]
        public decimal TotalPrice { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Скидка")]
        public decimal DiscountAmount { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Итоговая сумма")]
        public decimal FinalPrice { get; set; }

        [StringLength(20)]
        [Display(Name = "Статус оплаты")]
        public string PaymentStatus { get; set; } = "Ожидание";

        [StringLength(30)]
        [Display(Name = "Метод оплаты")]
        public string PaymentMethod { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Дата создания")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [Display(Name = "Дата выдачи")]
        public DateTime? PickupDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Дата завершения")]
        public DateTime? CompletedAt { get; set; }

        [StringLength(500)]
        [Display(Name = "Примечания")]
        public string Notes { get; set; }

        [Display(Name = "Кто изменил")]
        public int? LastModifiedBy { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Дата изменения")]
        public DateTime? LastModifiedAt { get; set; }

        // Навигационные свойства
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("CategoryId")]
        public virtual ItemCategory Category { get; set; }

        [ForeignKey("LastModifiedBy")]
        public virtual User ModifiedBy { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<OrderHistory> OrderHistories { get; set; }
    }
}