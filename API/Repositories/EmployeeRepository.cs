using API.Contracts;
using API.Data;
using API.Models;
using API.Utilities.Enums;

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

        public Employee GetAdminEmployee()
        {
            return _context.Employees
                .FirstOrDefault(e => e.Account.Role.Name == "admin");
        }

        public int GetCountIdle()
        {
            int idleEmployeeCount = _context.Employees
                .Count(e => e.StatusEmployee == StatusEmployee.idle);

            return idleEmployeeCount;
        }

        public int GetCaountHired()
        {
            int hiredEmployeeCount = _context.Employees
                 .Count(e => e.StatusEmployee == StatusEmployee.onsite);

            return hiredEmployeeCount;
        }
    }
}
