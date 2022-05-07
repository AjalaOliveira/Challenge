using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Challenge.API.ViewModels
{
    public class PersonViewModel : CommonDataViewModel
    {
        [JsonPropertyName("conjuge")]
        public SpouseViewModel Spouse { get; set; }

        [Required(ErrorMessage = "O campo '{0}' é obrigatório!")]
        [JsonPropertyName("dadosFamilia")]
        [Display(Name = "dadosFamilia")]
        public FamilyDataViewModel FamilyData { get; set; }
    }
}