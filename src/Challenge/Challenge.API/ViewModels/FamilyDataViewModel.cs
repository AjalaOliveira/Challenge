using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Challenge.API.ViewModels
{
    public class FamilyDataViewModel
    {
        [Required(ErrorMessage = "O campo '{0}' é obrigatório!")]
        [JsonPropertyName("rendaFamiliar")]
        [Display(Name = "rendaFamiliar")]
        public decimal? TotalIncome { get; set; }

        [JsonPropertyName("dependentes")]
        public List<DependentViewModel> Dependents { get; set; }
    }
}