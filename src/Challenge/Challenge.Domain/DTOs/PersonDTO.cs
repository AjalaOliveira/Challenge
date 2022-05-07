using System.Text.Json.Serialization;

namespace Challenge.Domain.DTOs
{
    public class PersonDTO : CommonDataDTO
    {
        public int Score { get; set; }
        public SpouseDTO Spouse { get; set; }
        public FamilyDataDTO FamilyData { get; set; }
    }
}