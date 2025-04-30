
namespace backend.Core.Entities
{
  public class RejectionReason
  {
    public Guid Id { get; set; }
    public required string reason { get; set; }
    public DateTime rejectionTime = DateTime.Now;
  }
}
