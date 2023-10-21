using API.DTOs.Salaries;
using FluentValidation;

namespace API.Utilities.Validations.Salaries
{
    public class SalaryValidator : AbstractValidator<SalaryDto>
    {
        public SalaryValidator()
        {
            RuleFor(e => e.Guid)
                .NotEmpty();

            RuleFor(e => e.BasicSalary)
               .NotEmpty();


        }
    }
}
