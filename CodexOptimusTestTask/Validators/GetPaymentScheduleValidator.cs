using CodexOptimusTestTask.Models.Dtos;
using FluentValidation;

namespace CodexOptimusTestTask.Validators
{
    /// <summary>
    /// Валидатор входных данных запроса GetLoanPaymentsSchedule
    /// </summary>
    public class GetPaymentScheduleValidator : AbstractValidator<GetLoanPaymentsScheduleDto>
    {
        public GetPaymentScheduleValidator()
        {
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
            RuleFor(x => x.PaymentDay).LessThan(32).GreaterThan(0);
            RuleFor(x => x.LoanAmount).GreaterThan(0);
            RuleFor(x => x.InterestRate).GreaterThan(0);
        }
    }
}
