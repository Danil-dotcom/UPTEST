using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPTEST.Models
{
    /// <summary>
    /// Модель: История изменений заказов
    /// Таблица: OrderHistory
    /// </summary>
    public class OrderHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Тип действия")]
        public string ActionType { get; set; } // CREATE, UPDATE, DELETE, STATUS_CHANGE, PAYMENT

        [StringLength(50)]
        [Display(Name = "Поле")]
        public string FieldName { get; set; }

        [Display(Name = "Старое значение")]
        public string OldValue { get; set; }

        [Display(Name = "Новое значение")]
        public string NewValue { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата действия")]
        public DateTime ActionDate { get; set; } = DateTime.Now;

        [StringLength(45)]
        [Display(Name = "IP адрес")]
        public string IPAddress { get; set; }

        // Навигационные свойства
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}