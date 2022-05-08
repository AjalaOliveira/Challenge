using Challenge.Domain.Config;
using System.ComponentModel.DataAnnotations;

namespace Challenge.API.ViewModels
{
    public class PersonViewModel : CommonDataViewModel
    {
        public int Score { get; set; }

        public SpouseViewModel Spouse { get; set; }

        [Required(ErrorMessage = ErrorMessage.FamilyDataError)]
        public FamilyDataViewModel FamilyData { get; set; }
    }
}