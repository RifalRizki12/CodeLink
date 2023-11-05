using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validations.Accounts
{
    public class AccountValidator : AbstractValidator<AccountDto>
    {
        // Konstruktor kelas CreateAccountValidator
        public AccountValidator()
        {
            // Aturan validasi untuk properti 'Password' dalam objek CreateAccountDto
            RuleFor(a => a.Password)
                .NotEmpty()         // Properti tidak boleh kosong
                .MinimumLength(8) // Panjang minimal 8 karakter
                .Matches(@"[A-Z]+") //harus berisi min 1 huruf kapital
                .Matches(@"[a-z]+");//harus berisi min 1 huruf lowercase

            // Aturan validasi untuk properti 'IsUsed' dalam objek CreateAccountDto
            RuleFor(a => a.IsUsed)
                .NotEmpty();        // Properti tidak boleh kosong

            RuleFor(a => a.Status)
                .NotEmpty();        // Properti tidak boleh kosong

            RuleFor(a => a.RoleGuid)
                .NotEmpty();

            RuleFor(a=> a.Guid)
                .NotEmpty();
        }
    }
}
