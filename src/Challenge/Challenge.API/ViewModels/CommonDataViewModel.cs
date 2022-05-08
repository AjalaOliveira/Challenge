using Challenge.Domain.Config;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Challenge.API.ViewModels
{
    public class CommonDataViewModel
    {
        [Required(ErrorMessage = ErrorMessage.FullNameError)]
        [StringLength(100, ErrorMessage = "O campo '{0}' precisa ter entre '{2}' e '{1}' caracteres!", MinimumLength = 5)]
        [JsonPropertyName("nomeCompleto")]
        public string FullName { get; set; }

        [Required(ErrorMessage = ErrorMessage.DocumentError)]
        [StringLength(15, ErrorMessage = "O campo '{0}' precisa ter entre '{2}' e '{1}' caracteres!", MinimumLength = 3)]
        [JsonPropertyName("numeroDocumento")]
        public string Document { get; set; }
    }
}
