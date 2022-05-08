using Challenge.Domain.Config;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Challenge.API.ViewModels
{
    public class DependentViewModel : CommonDataViewModel
    {
        [Required(ErrorMessage = ErrorMessage.BirthDateError)]
        [JsonPropertyName("dataNascimento")]
        public DateTime? BirthDate { get; set; }
    }
}