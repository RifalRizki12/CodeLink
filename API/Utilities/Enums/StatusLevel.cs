using System.ComponentModel.DataAnnotations;

namespace API.Utilities.Enums
{
    public enum StatusLevel
    {
        Requested,
        Approved,
        Rejected,
        Canceled,
        [Display(Name = "Non Aktif")] NonAktif,
        Idle,
        [Display(Name = "On Going")] OnGoing
    }
}
