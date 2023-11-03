using API.DTOs.Accounts;
using FluentValidation;

namespace API.Utilities.Validations.Accounts
{
    public class UpdateIdleValidator : AbstractValidator<UpdateIdleDto>
    {
        public UpdateIdleValidator()
        {
            // Aturan validasi untuk properti 'FirstName' dalam objek EmployeeDto
            RuleFor(e => e.FirstName)
                .NotEmpty()  // Properti tidak boleh kosong
                .MinimumLength(3); 


            // Aturan validasi untuk properti 'Gender' dalam objek EmployeeDto
            RuleFor(e => e.Gender)
                .NotNull()     // Properti tidak boleh null
                .IsInEnum();   // Properti harus merupakan nilai dari enum yang valid

            RuleFor(e => e.PhoneNumber)
                 .NotEmpty().WithMessage("Phone Number tidak boleh kosong")
                 .MaximumLength(16)
                 .Matches(@"^0\d*$").WithMessage("Phone Number harus dimulai dengan 0 dan hanya berisi angka");

            // Aturan validasi untuk properti 'Email' dalam objek EmployeeDto
            RuleFor(e => e.Email)
                .NotEmpty() // Properti tidak boleh kosong, dengan pesan kustom jika tidak terpenuhi
                .EmailAddress().WithMessage("Format Email Salah");  // Properti harus merupakan alamat email yang valid, dengan pesan kustom jika tidak terpenuhi

            // Aturan validasi untuk properti 'PhoneNumber' dalam objek EmployeeDto

            RuleFor(e => e.StatusEmployee)
                .NotNull();

        }
    }
}
