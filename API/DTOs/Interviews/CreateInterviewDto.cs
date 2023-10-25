
using API.Models;
using static System.Net.Mime.MediaTypeNames;

namespace API.DTOs.Interviews
{
    public class CreateInterviewDto //untuk create interview oleh client
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public Guid EmployeeGuid { get; set; }
        public Guid OwnerGuid { get; set; }



        public static implicit operator Interview(CreateInterviewDto createInterviewDto)
        {
            return new Interview
            {
                OwnerGuid = createInterviewDto.OwnerGuid,
                Name = createInterviewDto.Name,
                Date = createInterviewDto.Date,
                EmployeeGuid = createInterviewDto.EmployeeGuid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }

        public static implicit operator Rating(CreateInterviewDto createInterviewDto)
        {
            return new Rating
            {
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now

            };
        }
    }
}
