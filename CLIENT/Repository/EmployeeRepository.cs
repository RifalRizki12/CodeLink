using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Models;
using API.Utilities.Handler;
using CLIENT.Contract;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace CLIENT.Repository
{
    public class EmployeeRepository : GeneralRepository<Employee, Guid>, IEmployeeRepository
    {

        public EmployeeRepository(string request = "Employee/") : base(request)
        {

        }

        public async Task<ResponseOKHandler<IEnumerable<EmployeeDetailDto>>> GetDetailIdle()
        {
            // Ganti request ke endpoint yang sesuai
            var requestUrl = "detailsIdle";

            using (var response = await httpClient.GetAsync(request + requestUrl))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<IEnumerable<EmployeeDetailDto>>>(apiResponse);
                return entityVM;
            }
        }

        public async Task<ResponseOKHandler<UpdateIdleDto>> UpdateIdle(UpdateIdleDto employeeDto)
        {
            string requestUrl = "updateIdle"; // Sesuaikan dengan URL endpoint yang benar
            var content = new StringContent(JsonConvert.SerializeObject(employeeDto), Encoding.UTF8, "application/json");

            using (var response = await httpClient.PutAsync(request + requestUrl, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<UpdateIdleDto>>(apiResponse);
                    return entityVM;
                }
                else
                {
                    throw new HttpRequestException($"HTTP error: {response.StatusCode}");
                }
            }
        }

        public async Task<ResponseOKHandler<Employee>> RegisterIdle(RegisterIdleDto registrationDto)
        {
            string requestUrl = "registerIdle"; // Sesuaikan dengan URL endpoint yang benar
            var content = new StringContent(JsonConvert.SerializeObject(registrationDto), Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync(request + requestUrl, content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<Employee>>(apiResponse);
                return entityVM;
            }
        }

        public async Task<ResponseOKHandler<IEnumerable<ClientDetailDto>>> GetDetailClient()
        {
            // Ganti request ke endpoint yang sesuai
            var requestUrl = "allClient-details";

            using (var response = await httpClient.GetAsync(request + requestUrl))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<IEnumerable<ClientDetailDto>>>(apiResponse);
                return entityVM;
            }
        }

        public async Task<ResponseOKHandler<UpdateClientDto>> UpdateClient(Guid guid, UpdateClientDto clientDto)
        {
            string requestUrl = "updateClient"; // Sesuaikan dengan URL endpoint yang benar
            var content = new StringContent(JsonConvert.SerializeObject(clientDto), Encoding.UTF8, "application/json");

            using (var response = await httpClient.PutAsync(request + requestUrl, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<UpdateClientDto>>(apiResponse);
                    return entityVM;
                }
                else
                {
                    throw new HttpRequestException($"HTTP error: {response.StatusCode}");
                }
            }

        }

    }
}
