using API.DTOs.Companies;
using API.DTOs.Employees;
using FluentValidation;

namespace API.Utilities.Validations.Companies
{
    public class CompanyValidator : AbstractValidator<CompanyDto>
    {
        public CompanyValidator()
        {
            
            RuleFor(e => e.Name)
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(e => e.Address)
                .NotEmpty()
                .MinimumLength(4);
        }
    }
}
