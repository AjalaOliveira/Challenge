using Challenge.Domain.Config;
using System.ComponentModel.DataAnnotations;

namespace Challenge.API.ViewModels
{
    public class ApplicantViewModel : CommonDataViewModel
    {
        public int Score { get; set; }

        [Required(ErrorMessage = ErrorMessage.SpouseError)]
        public SpouseViewModel Spouse { get; set; }

        [Required(ErrorMessage = ErrorMessage.FamilyDataError)]
        public FamilyDataViewModel FamilyData { get; set; }
    }
}