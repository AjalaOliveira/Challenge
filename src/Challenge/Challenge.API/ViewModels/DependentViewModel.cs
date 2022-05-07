using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Challenge.API.ViewModels
{
    public class DependentViewModel : CommonDataViewModel
    {
        [Required(ErrorMessage = "O campo '{0}' é obrigatório!")]
        [JsonPropertyName("dataNascimento")]
        [Display(Name = "dataNascimento")]
        public DateTime? BirthDate { get; set; }
    }
}