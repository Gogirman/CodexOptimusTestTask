using CodexOptimusTestTask.Models;
using CodexOptimusTestTask.Models.Dtos;
using CodexOptimusTestTask.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodexOptimusTestTaskTests.Services
{
    [TestFixture]
    public class LoanPaymentsServiceTests
    {
        private ILoanPaymentsService _loanPaymentsService;

        [SetUp]
        public void SetUp()
        {
            _loanPaymentsService = new LoanPaymentsService();
        }

        [Test]
        public void GetCreditPaymentsScheduleAsync_AnnuityPayments_ReturnsExpectedSchedule()
        {
            // Arrange
            var loanAmount = 100000M;
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 4, 1);
            var interestRate = 10M;
            var paymentType = PaymentType.Annuity;
            var paymentDay = 1;

            var getPaymentScheduleDto = new GetLoanPaymentsScheduleDto
            {
                LoanAmount = loanAmount,
                StartDate = startDate,
                EndDate = endDate,
                InterestRate = interestRate,
                PaymentType = paymentType,
                PaymentDay = paymentDay
            };

            // Act
            var result = _loanPaymentsService.GetLoanPaymentsSchedule(getPaymentScheduleDto);

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(new DateTime(2023, 2, 1), result[0].PaymentDate);
            Assert.AreEqual(33123.30, Math.Round(result[0].PrincipalAmount, 2));
            Assert.AreEqual(767.12, Math.Round(result[0].InterestAmount, 2));
            Assert.AreEqual(66876.70, Math.Round(result[0].RemainingBalance, 2));
            Assert.AreEqual(new DateTime(2023, 3, 1), result[1].PaymentDate);
            Assert.AreEqual(33322.43, Math.Round(result[1].PrincipalAmount, 2));
            Assert.AreEqual(567.99, Math.Round(result[1].InterestAmount, 2));
            Assert.AreEqual(33554.27, Math.Round(result[1].RemainingBalance, 2));
            Assert.AreEqual(new DateTime(2023, 4, 1), result[2].PaymentDate);
            Assert.AreEqual(33675.01, Math.Round(result[2].PrincipalAmount, 2));
            Assert.AreEqual(275.79, Math.Round(result[2].InterestAmount, 2));
            Assert.AreEqual(0, Math.Round(result[2].RemainingBalance, 2));
        }

        [Test]
        public void GetCreditPaymentsScheduleAsync_DifferentiatedPayments_ReturnsExpectedSchedule()
        {
            // Arrange
            var loanAmount = 100000M;
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 4, 1);
            var interestRate = 10M;
            var paymentType = PaymentType.Differentiated;
            var paymentDay = 1;

            var getPaymentScheduleDto = new GetLoanPaymentsScheduleDto
            {
                LoanAmount = loanAmount,
                StartDate = startDate,
                EndDate = endDate,
                InterestRate = interestRate,
                PaymentType = paymentType,
                PaymentDay = paymentDay
            };

            // Act
            var result = _loanPaymentsService.GetLoanPaymentsSchedule(getPaymentScheduleDto);

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(new DateTime(2023, 2, 1), result[0].PaymentDate);
            Assert.AreEqual(33333.33, Math.Round(result[0].PrincipalAmount, 2));
            Assert.AreEqual(767.12, Math.Round(result[0].InterestAmount, 2));
            Assert.AreEqual(66666.67, Math.Round(result[0].RemainingBalance, 2));
            Assert.AreEqual(new DateTime(2023, 3, 1), result[1].PaymentDate);
            Assert.AreEqual(33333.33, Math.Round(result[1].PrincipalAmount, 2));
            Assert.AreEqual(566.21, Math.Round(result[1].InterestAmount, 2));
            Assert.AreEqual(33333.33, Math.Round(result[1].RemainingBalance, 2));
            Assert.AreEqual(new DateTime(2023, 4, 1), result[2].PaymentDate);
            Assert.AreEqual(33333.33, Math.Round(result[2].PrincipalAmount, 2));
            Assert.AreEqual(273.97, Math.Round(result[2].InterestAmount, 2));
            Assert.AreEqual(0, Math.Round(result[2].RemainingBalance, 2));
        }

        [Test]
        public void GetCreditPaymentsScheduleAsync_EndDateSameAsStartDatePlusOneMonth_ReturnsSinglePayment()
        {
            // Arrange
            var getPaymentScheduleDto = new GetLoanPaymentsScheduleDto
            {
                LoanAmount = 100000,
                StartDate = new DateTime(2023, 1, 1),
                EndDate = new DateTime(2023, 2, 1),
                InterestRate = 10,
                PaymentType = PaymentType.Annuity,
                PaymentDay = 1
            };

            // Act
            var result = _loanPaymentsService.GetLoanPaymentsSchedule(getPaymentScheduleDto);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(new DateTime(2023, 2, 1), result[0].PaymentDate);
            Assert.AreEqual(100132.42, Math.Round(result[0].PrincipalAmount, 2));
            Assert.AreEqual(767.12, Math.Round(result[0].InterestAmount, 2));
            Assert.AreEqual(0, Math.Round(result[0].RemainingBalance, 2));
        }

        [Test]
        public void GetCreditPaymentsScheduleAsync_RemainingBalanceLessThanAnnuityPayment_ReturnsLastPaymentEqualsZeroAndThisPaymentAddedToAnnuityPayment()
        {
            // Arrange
            var getPaymentScheduleDto = new GetLoanPaymentsScheduleDto
            {
                LoanAmount = 100000,
                StartDate = new DateTime(2023, 1, 1),
                EndDate = new DateTime(2023, 6, 1),
                InterestRate = 10,
                PaymentType = PaymentType.Annuity,
                PaymentDay = 1
            };

            // Act
            var result = _loanPaymentsService.GetLoanPaymentsSchedule(getPaymentScheduleDto);

            // Assert
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(new DateTime(2023, 5, 1), result[3].PaymentDate);
            Assert.IsTrue(result[4].PrincipalAmount > result[0].PrincipalAmount);
            Assert.IsTrue(result[4].RemainingBalance == 0);
        }
    }
}
