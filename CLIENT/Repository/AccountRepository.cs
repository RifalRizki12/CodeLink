using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Models;
using API.Utilities.Handler;
using CLIENT.Contract;
using CLIENT.Models;
using CLIENT.Repository;
using FluentValidation;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;


namespace CLIENT.Repository
{
    public class AccountRepository : GeneralRepository<AccountDto, Guid>, IAccountRepository
    {
        public AccountRepository(string request = "Account/") : base(request)
        {

        }

        public async Task<ResponseOKHandler<ChangePasswordDto>> ChangePassword(string email, ChangePasswordDto changePsswdDto)
        {
            string requestUrl = "change-password";
            ResponseOKHandler<ChangePasswordDto> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(changePsswdDto), Encoding.UTF8, "application/json");
            using (var response = httpClient.PutAsync(request + requestUrl, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<ChangePasswordDto>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseOKHandler<ForgotPasswordDto>> ForgotPassword(string email, ForgotPasswordDto forgotDto)
        {
            string requestUrl = "forgot-password/" + email;
            ResponseOKHandler<ForgotPasswordDto> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(forgotDto), Encoding.UTF8, "application/json");
            using (var response = httpClient.PutAsync(request + requestUrl, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<ForgotPasswordDto>>(apiResponse);
            }
            return entityVM;
        }

        /*public async Task<ResponseOKHandler<TokenDto>> Login(LoginDto login)
        {
            string jsonEntity = JsonConvert.SerializeObject(login);
            StringContent content = new StringContent(jsonEntity, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync($"{request}login", content))
            {
                response.EnsureSuccessStatusCode();
                string apiResponse = await response.Content.ReadAsStringAsync();
                var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<TokenDto>>(apiResponse);
                return entityVM;
            }
        }*/

        public async Task<ResponseOKHandler<TokenDto>> Login(LoginDto login)
        {
            string jsonEntity = JsonConvert.SerializeObject(login);
            StringContent content = new StringContent(jsonEntity, Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync($"{request}login", content))
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    // Respons status adalah 400 (Bad Request), baca pesan kesalahan validasi
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var validationErrors = JsonConvert.DeserializeObject<ResponseErrorHandler>(apiResponse).Error;

                    // Handle pesan kesalahan validasi di sini
                    string validationErrorMessage = "Validasi gagal: " + string.Join(", ", validationErrors);

                    // Anda dapat melemparkan pengecualian dengan pesan kesalahan validasi
                    throw new ValidationException(validationErrorMessage);

                    // Atau jika Anda ingin mengembalikan pesan kesalahan kepada pengguna, Anda bisa mengembalikan respons khusus
                    // Misalnya: return new ResponseErrorHandler { Code = StatusCodes.Status400BadRequest, Status = HttpStatusCode.BadRequest.ToString(), Message = validationErrorMessage };
                }
                else
                {
                    // Respons status lainnya (status 200 OK), proses respons seperti biasa
                    response.EnsureSuccessStatusCode();
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<TokenDto>>(apiResponse);
                    return entityVM;
                }
            }
        }

    }
}
