using API.DTOs.Accounts;
using API.DTOs.Ratings;
using API.Models;
using API.Utilities.Handler;
using CLIENT.Contract;
using Newtonsoft.Json;
using System.Text;

namespace CLIENT.Repository
{
    public class RatingRepository : GeneralRepository<RatingDto, Guid>, IRatingRepository
    {

        public RatingRepository(string request = "Rating/") : base(request)
        {

        }
        /*public async Task<ResponseOKHandler<RatingDto>> UpdateRating(Guid guid, RatingDto rating)
        {

            ResponseOKHandler<RatingDto> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(rating), Encoding.UTF8, "application/json");
            using (var response = httpClient.PutAsync(request , content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseOKHandler<RatingDto>>(apiResponse);
            }
            return entityVM;
        }*/
    }
}
