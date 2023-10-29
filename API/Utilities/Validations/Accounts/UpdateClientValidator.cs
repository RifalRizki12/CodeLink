/*using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validations.Accounts
{
    public class UpdateClientValidator : AbstractValidator<UpdateClientDto>
    {
        public UpdateClientValidator()
        {
            // Aturan validasi untuk properti 'FirstName' dalam objek EmployeeDto
            RuleFor(e => e.FirstName)
                .NotEmpty()
                .MinimumLength(3);  // Properti tidak boleh kosong

            // Aturan validasi untuk properti 'Gender' dalam objek EmployeeDto
            RuleFor(e => e.Gender)
                .NotNull()     // Properti tidak boleh null
                .IsInEnum();   // Properti harus merupakan nilai dari enum yang valid

            // Aturan validasi untuk properti 'Email' dalam objek EmployeeDto
            RuleFor(e => e.Email)
                .NotEmpty() // Properti tidak boleh kosong, dengan pesan kustom jika tidak terpenuhi
                .EmailAddress().WithMessage("Format Email Salah");  // Properti harus merupakan alamat email yang valid, dengan pesan kustom jika tidak terpenuhi

            // Aturan validasi untuk properti 'PhoneNumber' dalam objek EmployeeDto
            RuleFor(e => e.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number tidak boleh kosong")         // Properti tidak boleh kosong
                .MaximumLength(16);

            RuleFor(e => e.NameCompany)
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(e => e.AddressCompany)
                .NotEmpty()
                .MinimumLength(4);

*//*            RuleFor(e => e.Password)
                .NotEmpty()         // Properti tidak boleh kosong
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
*//*
        }
    }
}
*/