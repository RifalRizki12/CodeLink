using API.DTOs.Salaries;
using FluentValidation;

namespace API.Utilities.Validations.Salaries
{
    public class CreateSalaryValidator : AbstractValidator<CreateSalaryDto>
    {
        public CreateSalaryValidator()
        {

            RuleFor(e => e.BasicSalary)
               .NotEmpty();


        }
    }
}
