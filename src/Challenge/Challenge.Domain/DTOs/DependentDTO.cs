using System;
using System.Text.Json.Serialization;

namespace Challenge.Domain.DTOs
{
    public class DependentDTO : CommonDataDTO
    {
        public DateTime BirthDate { get; set; }
    }
}