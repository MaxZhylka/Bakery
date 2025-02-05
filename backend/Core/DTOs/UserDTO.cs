namespace backend.Core.DTOs
{
  public class UserDTO
  {
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }

  }

}
