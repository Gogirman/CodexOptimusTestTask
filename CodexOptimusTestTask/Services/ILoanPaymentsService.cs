using CodexOptimusTestTask.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodexOptimusTestTask.Services
{
    /// <summary>
    /// Сервис для составления графиков выплат по кредиту
    /// </summary>
    public interface ILoanPaymentsService
    {
        /// <summary>
        /// Расчёт графика ежемесячных платежей
        /// </summary>
        List<LoanPaymentsScheduleResultDto> GetLoanPaymentsSchedule(GetLoanPaymentsScheduleDto getPaymentScheduleDto);
    }
}
