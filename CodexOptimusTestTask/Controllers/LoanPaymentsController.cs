using CodexOptimusTestTask.Models.Dtos;
using CodexOptimusTestTask.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CodexOptimusTestTask.Controllers
{
    /// <summary>
    /// Контроллер для работы с кредитами
    /// </summary>
    [Route("loans")]
    public class LoanPaymentsController : Controller
    {
        private readonly ILoanPaymentsService _loanPaymentsService;
        private readonly IValidator<GetLoanPaymentsScheduleDto> _validator;

        public LoanPaymentsController(ILoanPaymentsService loanPaymentsService, 
            IValidator<GetLoanPaymentsScheduleDto> validator)
        {
            _loanPaymentsService = loanPaymentsService;
            _validator = validator;
        }

        [HttpPost]
        [Route("paymentschedule")]
        public async Task<IActionResult> GetLoanPaymentsSchedule([FromBody]GetLoanPaymentsScheduleDto getPaymentScheduleDto)
        {
            try
            {
                await _validator.ValidateAndThrowAsync(getPaymentScheduleDto);
                var result = _loanPaymentsService.GetLoanPaymentsSchedule(getPaymentScheduleDto);

                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
