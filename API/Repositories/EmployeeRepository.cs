using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
    {
        private readonly CodeLinkDbContext _context;
        public EmployeeRepository(CodeLinkDbContext context) : base(context)
        {
            _context = context;
        }

        public Employee GetByEmployeeEmail(string employeeEmail)
        {
            // Implementasi metode GetByEmployeeEmail di sini
            // Menggunakan LINQ untuk mencari karyawan berdasarkan email
            return _context.Employees.FirstOrDefault(employee => employee.Email == employeeEmail);
        }
    }
}
