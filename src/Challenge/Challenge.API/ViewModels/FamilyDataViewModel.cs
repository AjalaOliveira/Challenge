using Challenge.Domain.Config;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Challenge.API.ViewModels
{
    public class FamilyDataViewModel
    {
        [Required(ErrorMessage = ErrorMessage.TotalIncomeError)]
        public decimal? TotalIncome { get; set; }

        public List<DependentViewModel> Dependents { get; set; }
    }
}