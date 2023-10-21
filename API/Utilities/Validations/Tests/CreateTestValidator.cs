﻿using API.DTOs.Tests;
using FluentValidation;

namespace API.Utilities.Validations.Test
{
    public class CreateTestValidator : AbstractValidator<CreateTestDto>
    {
        public CreateTestValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(e => e.Date)
                .NotEmpty();

            RuleFor(e => e.EmployeeGuid)
                .NotEmpty();
        }
    }
}
