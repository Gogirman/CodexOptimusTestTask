using CodexOptimusTestTask.Models;
using CodexOptimusTestTask.Models.Dtos;
using System;
using System.Collections.Generic;

namespace CodexOptimusTestTask.Services
{
    /// <summary>
    /// Сервис для составления графиков выплат по кредиту
    /// </summary>
    public class LoanPaymentsService : ILoanPaymentsService
    {
        /// <summary>
        /// Расчёт графика ежемесячных платежей
        /// </summary>
        public List<LoanPaymentsScheduleResultDto> GetLoanPaymentsSchedule(GetLoanPaymentsScheduleDto getPaymentScheduleDto)
        {
            var paymentType = getPaymentScheduleDto.PaymentType;

            if (paymentType == PaymentType.Annuity)
            {
                return GetAnnuityPaymentsSchedule(getPaymentScheduleDto);
            }
            else if (paymentType == PaymentType.Differentiated)
            {
                return GetDifferentiatedPaymentsSchedule(getPaymentScheduleDto);
            }

            throw new ArgumentException("Unknown payment type");
        }

        /// <summary>
        /// Расчёт аннуитетного графика
        /// </summary>
        private List<LoanPaymentsScheduleResultDto> GetAnnuityPaymentsSchedule(GetLoanPaymentsScheduleDto getPaymentScheduleDto)
        {
            var payments = new List<LoanPaymentsScheduleResultDto>();

            var loanAmount = getPaymentScheduleDto.LoanAmount;
            var startDate = getPaymentScheduleDto.StartDate;
            var paymentDay = getPaymentScheduleDto.PaymentDay;
            var endDate = getPaymentScheduleDto.EndDate;
            var interestRate = getPaymentScheduleDto.InterestRate / 100;

            var paymentDate = GetNextPaymentDate(startDate, paymentDay);
            var daysInMonth = DateTime.DaysInMonth(paymentDate.Year, paymentDate.Month);

            var remainingBalance = loanAmount;
            var annuityPayment = CalculateAnnuityPayment(loanAmount, interestRate, endDate, paymentDate);
            while (paymentDate <= endDate)
            {
                var interest = remainingBalance * interestRate * daysInMonth / 365;
                var principal = annuityPayment - interest;
                remainingBalance -= principal;
                if (remainingBalance < 0)
                {
                    principal += Math.Abs(remainingBalance);
                    remainingBalance = 0;
                }
                payments.Add(new LoanPaymentsScheduleResultDto 
                { 
                    PaymentDate = paymentDate, 
                    PrincipalAmount = principal,
                    InterestAmount = interest,
                    RemainingBalance = remainingBalance 
                });

                paymentDate = GetNextPaymentDate(paymentDate, paymentDay);
                daysInMonth = DateTime.DaysInMonth(paymentDate.Year, paymentDate.Month);
            }

            return payments;
        }

        /// <summary>
        /// Расчёт дифференцированного графика
        /// </summary>
        private List<LoanPaymentsScheduleResultDto> GetDifferentiatedPaymentsSchedule(GetLoanPaymentsScheduleDto getPaymentScheduleDto)
        {
            var payments = new List<LoanPaymentsScheduleResultDto>();

            var loanAmount = getPaymentScheduleDto.LoanAmount;
            var startDate = getPaymentScheduleDto.StartDate;
            var paymentDay = getPaymentScheduleDto.PaymentDay;
            var endDate = getPaymentScheduleDto.EndDate;
            var interestRate = getPaymentScheduleDto.InterestRate / 100;

            var paymentDate = GetNextPaymentDate(startDate, paymentDay);
            var daysInMonth = DateTime.DaysInMonth(paymentDate.Year, paymentDate.Month);

            var remainingBalance = loanAmount;
            while (paymentDate <= endDate)
            {
                var principal = loanAmount / ((endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month);
                var interest = remainingBalance * interestRate * daysInMonth / 365;
                remainingBalance -= principal;

                if (remainingBalance < 0)
                {
                    principal += Math.Abs(remainingBalance);
                    remainingBalance = 0;
                }
                payments.Add(new LoanPaymentsScheduleResultDto 
                { 
                    PaymentDate = paymentDate, 
                    PrincipalAmount = principal, 
                    InterestAmount = interest, 
                    RemainingBalance = remainingBalance
                });

                paymentDate = GetNextPaymentDate(paymentDate, paymentDay);
                daysInMonth = DateTime.DaysInMonth(paymentDate.Year, paymentDate.Month);
            }

            return payments;
        }

        /// <summary>
        /// Получение следующей даты расчёта
        /// </summary>
        private DateTime GetNextPaymentDate(DateTime date, int paymentDay)
        {
            date = date.AddMonths(1);

            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            
            if (paymentDay > daysInMonth)
            {
                paymentDay = daysInMonth;
            }

            date = new DateTime(date.Year, date.Month, paymentDay);
            return date;
        }

        /// <summary>
        /// Получение аннуитета
        /// </summary>
        private decimal CalculateAnnuityPayment(decimal loanAmount, decimal interestRate, 
            DateTime endDate, DateTime paymentDate)
        {
            var numberOfPayments = (endDate.Year - paymentDate.Year) * 12 + endDate.Month - paymentDate.Month + 1;
            numberOfPayments = numberOfPayments == 0 ? 1 : numberOfPayments;
            var rate = interestRate / 12;
            var denominator = (decimal)Math.Pow((double)(1 + rate), numberOfPayments);
            var annuity = loanAmount * (rate * denominator / (denominator - 1));
            return annuity;
        }
    }
}
