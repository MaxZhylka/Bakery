namespace backend.Core.DTOs
{
  public class UserCreateDTO
  {
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
  }

}
