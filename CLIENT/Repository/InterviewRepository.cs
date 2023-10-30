using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.DTOs.Interviews;
using API.Models;
using API.Utilities.Handler;
using CLIENT.Contract;
using CLIENT.Models;
using CLIENT.Repository;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;


namespace CLIENT.Repository
{
    public class InterviewRepository : GeneralRepository<InterviewDto, Guid>, IInterviewRepository
    {
        public InterviewRepository(string request = "Interview/") : base(request)
        {

        }

    }
}
