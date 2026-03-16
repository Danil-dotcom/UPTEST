using System.Collections.Generic;

namespace UPTEST.Models
{
    /// <summary>
    /// Статические классы для статусов (чтобы избежать ошибок)
    /// </summary>
    public static class OrderStatuses
    {
        public const string Accepted = "Принят";
        public const string InProgress = "В чистке";
        public const string Ready = "Готов";
        public const string Completed = "Выдан";
        public const string Cancelled = "Отменен";
        public const string Return = "Возврат";

        public static readonly List<string> All = new List<string>
        {
            Accepted, InProgress, Ready, Completed, Cancelled, Return
        };

        public static bool IsValid(string status)
        {
            return All.Contains(status);
        }
    }

    public static class OrderPriorities
    {
        public const string Normal = "Обычный";
        public const string Urgent = "Срочный";
        public const string VIP = "VIP";

        public static readonly List<string> All = new List<string>
        {
            Normal, Urgent, VIP
        };
    }

    public static class PaymentStatuses
    {
        public const string Pending = "Ожидание";
        public const string Paid = "Оплачен";
        public const string Partially = "Частично";
        public const string Refund = "Возврат";

        public static readonly List<string> All = new List<string>
        {
            Pending, Paid, Partially, Refund
        };
    }

    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string User = "User";

        public static readonly List<string> All = new List<string>
        {
            Admin, Manager, User
        };
    }

    public static class ActionTypes
    {
        public const string Create = "CREATE";
        public const string Update = "UPDATE";
        public const string Delete = "DELETE";
        public const string StatusChange = "STATUS_CHANGE";
        public const string Payment = "PAYMENT";

        public static readonly List<string> All = new List<string>
        {
            Create, Update, Delete, StatusChange, Payment
        };
    }
}