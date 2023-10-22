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

        public Employee GetAdminEmployee()
        {
            return _context.Employees
                .Where(e => e.Account.AccountRoles.Any(ar => ar.Role.Name == "admin"))
                .FirstOrDefault();
        }

        public int GetCountIdle()
        {
            int idleEmployeeCount = _context.Employees
                .Count(e => e.Status == "idle");

            return idleEmployeeCount;
        }

        public int GetCaountHired()
        {
            int hiredEmployeeCount = _context.Employees
                 .Count(e => e.Status == "onsite");

            return hiredEmployeeCount;
        }
    }
}
