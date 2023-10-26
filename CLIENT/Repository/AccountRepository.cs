using API.DTOs.Accounts;
using API.DTOs.Employees;
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
    public class AccountRepository : GeneralRepository<AccountDto, Guid>, IAccountRepository
    {
        public AccountRepository(string request = "Account/") : base(request)
        {

        }

        public async Task<ResponseOKHandler<TokenDto>> Login(LoginDto login)
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
        }
    }
}
