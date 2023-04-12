using System;

namespace CodexOptimusTestTask.Models.Dtos
{
    /// <summary>
    /// Результат работы метода GetLoanPaymentsSchedule
    /// </summary>
    public class LoanPaymentsScheduleResultDto
    {
        /// <summary>
        /// Дата платежа
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Сумма платежа по основному долгу
        /// </summary>
        public decimal PrincipalAmount { get; set; }

        /// <summary>
        /// Сумма платежа по процентам
        /// </summary>
        public decimal InterestAmount { get; set; }

        /// <summary>
        /// Сумма оставшегося после платежа основного долга
        /// </summary>
        public decimal RemainingBalance { get; set; }
    }
}
