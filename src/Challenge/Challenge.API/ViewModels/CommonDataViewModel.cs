using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Challenge.API.ViewModels
{
    public class CommonDataViewModel
    {
        [Required(ErrorMessage = "O campo '{0}' é obrigatório!")]
        [StringLength(100, ErrorMessage = "O campo '{0}' precisa ter entre '{2}' e '{1}' caracteres!", MinimumLength = 5)]
        [JsonPropertyName("nomeCompleto")]
        [Display(Name = "nomeCompleto")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "O campo '{0}' é obrigatório!")]
        [StringLength(15, ErrorMessage = "O campo '{0}' precisa ter entre '{2}' e '{1}' caracteres!", MinimumLength = 3)]
        [JsonPropertyName("numeroDocumento")]
        [Display(Name = "numeroDocumento")]
        public string Document { get; set; }
    }
}
