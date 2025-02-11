using backend.Core.Enums;

namespace backend.Core.Models
{
    public class UserActionLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Operations Operation { get; set; }
        public required string Details { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
