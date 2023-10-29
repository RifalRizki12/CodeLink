using API.Models;
using API.Utilities.Enums;
using System.Reflection.Metadata;

namespace API.DTOs.Employees
{
    public class ChartDto
    {
        public int HiredEmployeesCount { get; set; }
        public int IdleEmployeesCount { get; set; }
        public int CompaniesCount { get; set; }
                
    }

}
