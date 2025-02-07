
using backend.Core.Enums;

namespace Core.Exceptions
{
  public class UserNotFoundException : Exception
  {
    public UserNotFoundException(Guid id)
        : base($"User with ID {id} was not found") { }
    public UserNotFoundException(string email)
        : base($"User with such email: {email} not found") { }
  }

  public class OrderNotFoundException : Exception
  {
    public OrderNotFoundException(Guid id)
        : base($"Order with ID {id} was not found") { }
  }
  public class ProductNotFoundException : Exception
  {
    public ProductNotFoundException(Guid id)
        : base($"Product with ID {id} was not found") { }
  }
  public class UserAlreadyExistsException : Exception
  {
    public UserAlreadyExistsException(string email)
        : base($"User with email {email} already exists") { }
  }

  public class DatabaseOperationException : Exception
  {
    public DatabaseOperationException(Operations operation, Exception inner)
        : base($"Database error during {operation}", inner) { }
  }
  public class UserNotFoundByRefreshException : Exception
  {
    public UserNotFoundByRefreshException()
        : base("User with such refresh token not found") { }
  }
}