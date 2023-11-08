using API.Models;
using API.Utilities.Enums;
using System.Reflection.Metadata;

namespace API.DTOs.Employees
{
    public class ChartDto
    {
        public int HiredEmployeesCount { get; set; }
        public int IdleEmployeesCount { get; set; }
        public int AdminEmployeesCount { get; set; }
        public int CompaniesCount { get; set; }
        public int RequestedAccountCount { get; set; }
        public int CanceledAccountCount { get; set; }
        public int ApprovedAccountCount { get; set; }
        public int RejectedAccountCount { get; set; }
        public int NonAktifAccountCount { get; set; }
        public int LolosInterviewCount { get; set; }
        public int TidakLolosInterviewCount { get; set; }
        public int ContarctTerminationInterviewCount { get; set; }
        public int EndContractInterviewCount { get; set; }
                
    }

}
