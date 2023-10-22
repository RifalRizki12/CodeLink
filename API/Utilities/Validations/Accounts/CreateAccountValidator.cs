﻿using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validations.Accounts
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountDto>
    {
        // Konstruktor kelas CreateAccountValidator
        public CreateAccountValidator()
        {
            // Aturan validasi untuk properti 'Password' dalam objek CreateAccountDto
            RuleFor(e => e.Password)
                .NotEmpty()         // Properti tidak boleh kosong
                .MinimumLength(8);  // Panjang minimal 8 karakter

            // Aturan validasi untuk properti 'IsUsed' dalam objek CreateAccountDto
            RuleFor(e => e.IsUsed)
                .NotEmpty();        // Properti tidak boleh kosong

            // Aturan validasi untuk properti 'ExpiredTime' dalam objek CreateAccountDto
           
        }
    }
}
