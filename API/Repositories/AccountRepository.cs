using API.Contracts;
using API.Data;
using API.Models;

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

    }
}
