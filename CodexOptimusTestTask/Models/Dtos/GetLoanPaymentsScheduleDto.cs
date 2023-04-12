using System;

namespace CodexOptimusTestTask.Models.Dtos
{
    /// <summary>
    /// Входные данные метода GetLoanPaymentsSchedule
    /// </summary>
    public record GetLoanPaymentsScheduleDto
    {
        /// <summary>
        /// Сумма кредита
        /// </summary>
        public decimal LoanAmount { get; init; }

        /// <summary>
        /// Дата выдачи кредита
        /// </summary>
        public DateTime StartDate { get; init; }

        /// <summary>
        /// Дата закрытия кредита
        /// </summary>
        public DateTime EndDate { get; init; }

        /// <summary>
        /// Процентная ставка
        /// </summary>
        public decimal InterestRate { get; init; }

        /// <summary>
        /// Тип графика (аннуитетный/дифференцированный)
        /// </summary>
        public PaymentType PaymentType { get; init; }

        /// <summary>
        /// День платежа
        /// </summary>
        public int PaymentDay { get; init; }
    }
}
