using Challenge.Domain.Config;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Challenge.API.ViewModels
{
    public class PersonViewModel : CommonDataViewModel
    {
        [JsonPropertyName("conjuge")]
        public SpouseViewModel Spouse { get; set; }

        [Required(ErrorMessage = ErrorMessage.FamilyDataError)]
        [JsonPropertyName("dadosFamilia")]
        public FamilyDataViewModel FamilyData { get; set; }
    }
}