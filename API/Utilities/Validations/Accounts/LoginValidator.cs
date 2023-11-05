using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validations.Accounts
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        // Konstruktor kelas CreateAccountValidator
        public LoginValidator()
        {
            // Aturan validasi untuk properti 'Password' dalam objek CreateAccountDto
            RuleFor(e => e.Email)
                .NotEmpty()         // Properti tidak boleh kosong
                .EmailAddress();  // Panjang minimal 8 karakter

         
            RuleFor(e => e.Password)
                .NotEmpty()
                .MinimumLength(8) // Panjang minimal 8 karakter
                .Matches(@"[A-Z]+") //harus berisi min 1 huruf kapital
                .Matches(@"[a-z]+");//harus berisi min 1 huruf lowercase

        }
    }
}
