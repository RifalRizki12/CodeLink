
using API.Models;
using static System.Net.Mime.MediaTypeNames;

namespace API.DTOs.Interviews
{
    public class AnnouncmentDto //dipake buat ngasih announcment lolos/ tidak
    {
        public Guid Guid { get; set; }
        public string Remarks { get; set; }
        public DateTime? StartContract {  get; set; }
        public DateTime? EndContract {  get; set; }
        public Guid EmployeeGuid { get; set; }
        public string FeedBack {  get; set; }
        public Guid OwnerGuid { get; set; }
       



        public static implicit operator Interview(AnnouncmentDto announceDto)
        {
            return new Interview
            {
                Guid = announceDto.Guid,
                EmployeeGuid = announceDto.EmployeeGuid,
                OwnerGuid = announceDto.OwnerGuid,
                Remarks = announceDto.Remarks,
                StartContract = announceDto.StartContract,
                EndContract = announceDto.EndContract,
                ModifiedDate = DateTime.Now
            };
        }

        public static implicit operator Rating(AnnouncmentDto announceDto)
        {
            return new Rating
            {
                Feedback = announceDto.FeedBack,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
                
            };
        }
    }
}
