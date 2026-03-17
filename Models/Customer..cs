using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPTEST.Models
{
    /// <summary>
    /// Модель: Клиенты (постоянные клиенты)
    /// Таблица: Customers
    /// </summary>
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "ФИО клиента обязательно")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "ФИО должно быть от 5 до 100 символов")]
        [Display(Name = "ФИО")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Телефон обязателен")]
        [StringLength(20)]
        [Phone(ErrorMessage = "Некорректный номер телефона")]
        [Display(Name = "Телефон")]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [StringLength(200)]
        [Display(Name = "Адрес")]
        public string? Address { get; set; }

        [StringLength(20)]
        [Display(Name = "Дисконтная карта")]
        public string? DiscountCard { get; set; }

        [Range(0, 30, ErrorMessage = "Скидка должна быть от 0 до 30%")]
        [Display(Name = "Скидка %")]
        public int DiscountPercent { get; set; } = 0;

        [Display(Name = "Всего заказов")]
        public int TotalOrders { get; set; } = 0;

        [Column(TypeName = "decimal(10, 2)")]
        [Display(Name = "Потрачено всего")]
        public decimal TotalSpent { get; set; } = 0;

        [DataType(DataType.DateTime)]
        [Display(Name = "Дата регистрации")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        [Display(Name = "Примечания")]
        public string? Notes { get; set; }

        // Навигационное свойство
        public virtual ICollection<Order>? Orders { get; set; }
    }
}