using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.DTOs.Interviews;
using API.Models;
using API.Utilities.Handler;
using CLIENT.Contract;
using CLIENT.Models;
using CLIENT.Repository;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace CLIENT.Repository
{
    public class InterviewRepository : GeneralRepository<Interview, Guid>, IInterviewRepository
    {
        public InterviewRepository(string request = "Interview/") : base(request)
        {

        }

        public async Task<ResponseOKHandler<IEnumerable<InterviewDto>>> GetAllInterview()
        {

            using (var response = await httpClient.GetAsync(request))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<IEnumerable<InterviewDto>>>(apiResponse);
                return entityVM;
            }
        }

        public async Task<ResponseOKHandler<Interview>> ScheduleUpdate(ScheduleInterviewDto scheduleUpdate)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(scheduleUpdate), Encoding.UTF8, "application/json");

            using (var response = await httpClient.PutAsync($"{request}ScheduleInterview", content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<Interview>>(apiResponse);
                    return entityVM;
                }
                else
                {
                    Console.WriteLine($"Request failed with status code {response.StatusCode}: {response.ReasonPhrase}");
                    Console.WriteLine($"Response Content: {apiResponse}");

                    // Here, you can choose to throw an exception or return a specific response based on the status code.
                    // Example: You can create a custom exception and throw it with detailed information.
                    // Or, you can return a specific response indicating the failure.

                    // Example of throwing a custom exception:
                    throw new Exception($"Request failed with status code {response.StatusCode}: {response.ReasonPhrase}");

                    // Example of returning a specific response:
                    // return new ResponseOKHandler<Interview> { Message = "Request failed", Success = false };
                }
            }
        }

    }
}
