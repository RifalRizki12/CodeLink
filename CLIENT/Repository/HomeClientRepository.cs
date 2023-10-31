using API.Models;
using CLIENT.Contract;

namespace CLIENT.Repository
{
    public class HomeClientRepository : GeneralRepository<Employee, Guid>, IHomeClientRepository
    {

        public HomeClientRepository(string request = "Employee/") : base(request)
        {

        }

    }
}
