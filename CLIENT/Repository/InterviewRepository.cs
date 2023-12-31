﻿using API.DTOs.Accounts;
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
        
        public async Task<ResponseOKHandler<IEnumerable<GetInterviewDto>>> GetByCompanyGuid(Guid guid)
        {
            string requestUrl = "GetAllByClientGuid/" + guid; // Sesuaikan dengan URL endpoint yang benar

            ResponseOKHandler<IEnumerable<GetInterviewDto>> entity = null;

            using (var response = await httpClient.GetAsync(request + requestUrl))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<ResponseOKHandler<IEnumerable<GetInterviewDto>>>(apiResponse);
            }
            return entity;
        }

        public async Task<ResponseOKHandler<IEnumerable<GetOnsiteDto>>> GetOnsite(Guid guid)
        {
            string requestUrl = "GetOnsite/" + guid; // Sesuaikan dengan URL endpoint yang benar

            ResponseOKHandler<IEnumerable<GetOnsiteDto>> entityVM = null;
            using (var response = await httpClient.GetAsync(request + requestUrl))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<IEnumerable<GetOnsiteDto>>>(apiResponse);
            }
            return entityVM;
        }  

        public async Task<ResponseOKHandler<IEnumerable<GetIdleHistoryDto>>> GetIdleHistory(Guid guid)
        {
            string requestUrl = "GetIdleHistory/" + guid; // Sesuaikan dengan URL endpoint yang benar

            ResponseOKHandler<IEnumerable<GetIdleHistoryDto>> entityVM = null;
            using (var response = await httpClient.GetAsync(request + requestUrl))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<IEnumerable<GetIdleHistoryDto>>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseOKHandler<AnnouncmentDto>> UpdateAnnouncement(Guid guid, AnnouncmentDto announcment)
        {
            string requestUrl = "Announcement";
            ResponseOKHandler<AnnouncmentDto> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(announcment), Encoding.UTF8, "application/json");
            using (var response = httpClient.PutAsync(request + requestUrl, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<AnnouncmentDto>>(apiResponse);
            }
            return entityVM;
        }

    }
}
