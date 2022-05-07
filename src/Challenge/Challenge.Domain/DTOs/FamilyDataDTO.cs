using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Challenge.Domain.DTOs
{
    public class FamilyDataDTO
    {
        public decimal TotalIncome { get; set; }
        public List<DependentDTO> Dependents { get; set; }
    }
}