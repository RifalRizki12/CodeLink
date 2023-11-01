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


        public async Task<ResponseOKHandler<ScheduleInterviewDto>> ScheduleUpdate(Guid guid, ScheduleInterviewDto scheduleUpdate)
        {
            string requestUrl = "ScheduleInterview";
            ResponseOKHandler<ScheduleInterviewDto> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(scheduleUpdate), Encoding.UTF8, "application/json");
            using (var response = httpClient.PutAsync(request + requestUrl, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<ScheduleInterviewDto>>(apiResponse);
            }
            return entityVM;
        }        
        
        public async Task<ResponseOKHandler<GetInterviewDto>> GetByCompanyGuid(Guid guid)
        {
            string requestUrl = "GetAllByClientGuid/"; // Sesuaikan dengan URL endpoint yang benar

            ResponseOKHandler<GetInterviewDto> entity = null;

            using (var response = await httpClient.GetAsync(request + requestUrl + guid))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<ResponseOKHandler<GetInterviewDto>>(apiResponse);
            }
            return entity;
        }
    }
}
