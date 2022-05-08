using Challenge.Domain.Config;
using System;
using System.ComponentModel.DataAnnotations;

namespace Challenge.API.ViewModels
{
    public class DependentViewModel : CommonDataViewModel
    {
        [Required(ErrorMessage = ErrorMessage.BirthDateError)]
        public DateTime? BirthDate { get; set; }
    }
}