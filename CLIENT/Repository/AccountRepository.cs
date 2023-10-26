using API.DTOs.Accounts;
using API.Models;
using API.Utilities.Handler;
using CLIENT.Contract;
using Newtonsoft.Json;
using System.Text;

namespace CLIENT.Repository
{
    public class AccountRepository : GeneralRepository<Employee, Guid>, IAccountRepository
    {

        public AccountRepository(string request = "Account/") : base(request)
        {

        }

      /*  public async Task<ResponseOKHandler<IEnumerable<Account>>> GetDetailIdle()
        {
            // Ganti request ke endpoint yang sesuai
            var requestUrl = "detailsIdle";

            using (var response = await httpClient.GetAsync(request + requestUrl))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<IEnumerable<Account>>>(apiResponse);
                return entityVM;
            }
        }

        public async Task<ResponseOKHandler<Account>> RegisterIdle(RegisterIdleDto registrationDto)
        {
            string requestUrl = "registerIdle"; // Sesuaikan dengan URL endpoint yang benar
            var content = new StringContent(JsonConvert.SerializeObject(registrationDto), Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync(request + requestUrl, content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                var entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<Account>>(apiResponse);
                return entityVM;
            }
        }*/

    }
}
