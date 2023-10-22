using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validations.Accounts
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        // Konstruktor kelas CreateAccountValidator
        public ChangePasswordValidator()
        {
            // Aturan validasi untuk properti 'Password' dalam objek CreateAccountDto
            RuleFor(e => e.Email)
                .NotEmpty()         // Properti tidak boleh kosong
                .EmailAddress().WithMessage("Format Email Salah");  // Panjang minimal 8 karakter

            // Aturan validasi untuk properti 'IsUsed' dalam objek CreateAccountDto
            RuleFor(e => e.Otp)
                .NotEmpty();        // Properti tidak boleh kosong
           
            RuleFor(e => e.NewPassword)
                .NotEmpty()
                .MinimumLength(8) // Panjang minimal 8 karakter
                .MaximumLength(16) //max lenght karakter 16
                .Matches(@"[A-Z]+") //harus berisi min 1 huruf kapital
                .Matches(@"[a-z]+");//harus berisi min 1 huruf lowercase

            RuleFor(e => e.ConfirmPassword)
                .NotEmpty()         // Properti tidak boleh kosong
                .MinimumLength(8) // Panjang minimal 8 karakter
                .MaximumLength(16) //max lenght karakter 16
                .Matches(@"[A-Z]+") //harus berisi min 1 huruf kapital
                .Matches(@"[a-z]+");//harus berisi min 1 huruf lowercase

        }
    }
}
