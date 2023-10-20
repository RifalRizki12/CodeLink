using API.DTOs.Employees;
using FluentValidation;

namespace API.Utilities.Validations.Employees
{
    public class EmployeeValidator : AbstractValidator<EmployeeDto>
    {
        public EmployeeValidator()
        {
            RuleFor(e => e.Guid)
                .NotEmpty();

            // Aturan validasi untuk properti 'FirstName' dalam objek EmployeeDto
            RuleFor(e => e.FirstName)
                .NotEmpty()
                .MinimumLength(3);  // Properti tidak boleh kosong

            // Aturan validasi untuk properti 'Gender' dalam objek EmployeeDto
            RuleFor(e => e.Gender)
                .NotNull()     // Properti tidak boleh null
                .IsInEnum();   // Properti harus merupakan nilai dari enum yang valid

            // Aturan validasi untuk properti 'HiringDate' dalam objek EmployeeDto
            RuleFor(e => e.HiringDate)
                .NotEmpty();  // Properti tidak boleh kosong

            // Aturan validasi untuk properti 'Email' dalam objek EmployeeDto
            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Tidak Boleh Kosong")  // Properti tidak boleh kosong, dengan pesan kustom jika tidak terpenuhi
                .EmailAddress().WithMessage("Format Email Salah");  // Properti harus merupakan alamat email yang valid, dengan pesan kustom jika tidak terpenuhi

            // Aturan validasi untuk properti 'PhoneNumber' dalam objek EmployeeDto
            RuleFor(e => e.PhoneNumber)
                .NotEmpty()         // Properti tidak boleh kosong
                .MaximumLength(16); // Panjang maksimal 20 karakter
        }
    }
}
