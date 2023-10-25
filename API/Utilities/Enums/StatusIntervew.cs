using System.ComponentModel.DataAnnotations;

namespace API.Utilities.Enums;

public enum StatusIntervew
{
    [Display(Name = "Lolos")]
    Lolos = 0,

    [Display(Name = "Tidak Lolos")]
    TidakLolos = 1,

    [Display(Name = "Contarct Termination")]
    ContarctTermination = 2,    
    
    [Display(Name = "End Of Contarct")]
    EndContract = 3,
}