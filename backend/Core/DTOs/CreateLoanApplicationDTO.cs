using System.Text.Json.Serialization;
using backend.Core.Enums;

namespace backend.Core.DTOs
{
    public class CreateLoanApplicationDTO
    {
        public Guid UserId { get; set; }

        public int Value { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LoanTerm Term { get; set; }

    }
}
