using API.Contracts;
using API.Data;
using API.Models;
using API.Utilities.Enums;

namespace API.Repositories
{
    public class AccountRepository : GeneralRepository<Account>, IAccountRepository
    {
        private readonly CodeLinkDbContext _context;

        // Konstruktor AccountRepository yang menerima BookingManagementDbContext
        public AccountRepository(CodeLinkDbContext context) : base(context)
        {
            _context = context;
        }

        // Metode untuk mendapatkan akun berdasarkan alamat email karyawan
        public Account GetByEmployeeEmail(string employeeEmail)
        {
            // Melakukan join antara tabel Akun (Account) dan Employee berdasarkan email Employee
            var account = _context.Accounts
                .Join(
                    _context.Employees,
                    account => account.Guid,
                    employee => employee.Guid,
                    (account, employee) => new
                    {
                        Account = account,
                        Employee = employee
                    }
                )
                // Memfilter hasil join berdasarkan alamat email karyawan
                .Where(joinResult => joinResult.Employee.Email == employeeEmail)
                // Memilih objek akun sebagai hasil akhir
                .Select(joinResult => joinResult.Account)
                .FirstOrDefault();

            return account;
        }
        
        public int GetCaountRequested()
        {
            int requestedAccountCount = _context.Accounts
                .Count(e => e.Status == StatusLevel.Requested);

            return requestedAccountCount;
        }

        public int GetCaountCanceled()
        {
            int canceledAccountCount = _context.Accounts
                .Count(e => e.Status == StatusLevel.Canceled);

            return canceledAccountCount;
        }

        public int GetCaountApproved()
        {
            int approvedAccountCount = _context.Accounts
                .Count(e => e.Status == StatusLevel.Approved);

            return approvedAccountCount;
        }
        public int GetCaountRejected()
        {
            int rejectedAccountCount = _context.Accounts
                .Count(e => e.Status == StatusLevel.Rejected);

            return rejectedAccountCount;
        }
        public int GetCaountNonAktif()
        {
            int nonAktifAccountCount = _context.Accounts
                .Count(e => e.Status == StatusLevel.NonAktif);

            return nonAktifAccountCount;
        }
    }
}
