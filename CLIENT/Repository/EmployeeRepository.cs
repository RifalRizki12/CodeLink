using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Models;
using API.Utilities.Handler;
using CLIENT.Contract;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http.Headers;

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

        public async Task<ResponseOKHandler<ChartDto>> GetDetailChart()
        {
            // Ganti requestUrl dengan endpoint yang sesuai
            var requestUrl = "GetChart";

            using (var response = await httpClient.GetAsync(request + requestUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<ChartDto>>(apiResponse);

                    return entityVM;
                }
                else
                {
                    // Handle error response
                    throw new HttpRequestException($"Request to {requestUrl} failed with status code {response.StatusCode}");
                }
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
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    foreach (var prop in registrationDto.GetType().GetProperties())
                    {
                        var value = prop.GetValue(registrationDto);
                        if (value != null)
                        {
                            if (value is IFormFile file)
                            {
                                var fileContent = new StreamContent(file.OpenReadStream())
                                {
                                    Headers =
                        {
                            ContentLength = file.Length,
                            ContentType = new MediaTypeHeaderValue(file.ContentType)
                        }
                                };
                                content.Add(fileContent, prop.Name, file.FileName);
                            }
                            else if (prop.Name == "Skills" && value is List<string> skills)
                            {
                                for (int i = 0; i < skills.Count; i++)
                                {
                                    content.Add(new StringContent(skills[i]), $"Skills[{i}]");
                                }
                            }
                            else
                            {
                                content.Add(new StringContent(value.ToString()), prop.Name);
                            }
                        }
                    }

                    using (var response = await httpClient.PostAsync($"{request}RegisterIdle", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<Employee>>(apiResponse);
                            return entityVM;
                        }
                        else
                        {
                            // Handle non-success status codes as needed
                            if (response.StatusCode == HttpStatusCode.UnsupportedMediaType)
                            {
                                Console.WriteLine("415 Unsupported Media Type - Ensure the server accepts JSON.");
                            }
                            else
                            {
                                Console.WriteLine($"Request failed with status code {response.StatusCode}: {response.ReasonPhrase}");
                            }
                            Console.WriteLine($"Response Content: {apiResponse}");
                            // You might want to return a specific response or throw an exception here
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex);
                throw; // Consider whether re-throwing the exception is the best course of action
            }
        }

        public async Task<ResponseOKHandler<Company>> RegisterClient(RegisterClientDto registrationCDto)
        {
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    foreach (var prop in registrationCDto.GetType().GetProperties())
                    {
                        var value = prop.GetValue(registrationCDto);
                        if (value != null)
                        {
                            if (value is IFormFile file)
                            {
                                var fileContent = new StreamContent(file.OpenReadStream())
                                {
                                    Headers =
                            {
                                ContentLength = file.Length,
                                ContentType = new MediaTypeHeaderValue(file.ContentType)
                            }
                                };
                                content.Add(fileContent, prop.Name, file.FileName);
                            }
                            else
                            {
                                content.Add(new StringContent(value.ToString()), prop.Name);
                            }
                        }
                    }

                    using (var response = await httpClient.PostAsync($"{request}RegisterClient", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<Company>>(apiResponse);
                            return entityVM;
                        }
                        else
                        {
                            // Handle non-success status codes as needed
                            if (response.StatusCode == HttpStatusCode.UnsupportedMediaType)
                            {
                                Console.WriteLine("415 Unsupported Media Type - Ensure the server accepts JSON.");
                            }
                            else
                            {
                                Console.WriteLine($"Request failed with status code {response.StatusCode}: {response.ReasonPhrase}");
                            }
                            Console.WriteLine($"Response Content: {apiResponse}");
                            // You might want to return a specific response or throw an exception here
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex);
                throw; // Consider whether re-throwing the exception is the best course of action
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
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    foreach (var prop in clientDto.GetType().GetProperties())
                    {
                        var value = prop.GetValue(clientDto);
                        if (value != null)
                        {
                            if (value is IFormFile file)
                            {
                                var fileContent = new StreamContent(file.OpenReadStream())
                                {
                                    Headers =
                    {
                        ContentLength = file.Length,
                        ContentType = new MediaTypeHeaderValue(file.ContentType)
                    }
                                };
                                content.Add(fileContent, prop.Name, file.FileName);
                            }

                        }
                        else
                        {
                            content.Add(new StringContent(value.ToString()), prop.Name);
                        }
                    }

                    
                    using (var response = await httpClient.PostAsync($"{request}updateClient/" + guid, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<UpdateClientDto>>(apiResponse);
                            return entityVM;
                        }
                        else
                        {
                            // Handle non-success status codes as needed
                            if (response.StatusCode == HttpStatusCode.UnsupportedMediaType)
                            {
                                Console.WriteLine("415 Unsupported Media Type - Ensure the server accepts JSON.");
                            }
                            else
                            {
                                Console.WriteLine($"Request failed with status code {response.StatusCode}: {response.ReasonPhrase}");
                            }
                            Console.WriteLine($"Response Content: {apiResponse}");
                            // You might want to return a specific response or throw an exception here
                            return null;
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex);
                throw; // Consider whether re-throwing the exception is the best course of action
            }

        }

        public async Task<ResponseOKHandler<ClientDetailDto>> GetGuidClient(Guid guid)
        {
            string requestUrl = "getByGuidClient/"; // Sesuaikan dengan URL endpoint yang benar

            ResponseOKHandler<ClientDetailDto> entity = null;

            using (var response = await httpClient.GetAsync(request + requestUrl + guid))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<ResponseOKHandler<ClientDetailDto>>(apiResponse);
            }
            return entity;
        }
    }
}
