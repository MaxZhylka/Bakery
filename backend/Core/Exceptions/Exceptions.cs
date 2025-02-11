
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

  public class OrderNotFoundException(Guid id) : Exception($"Order with ID {id} was not found") { }
  public class LogNotFoundException(Guid id) : Exception($"Log with such id: {id} not found") { }
  public class LogCreationException(): Exception($"Database didn't return any data, something went wrong") {}
  public class ProductNotFoundException(Guid id) : Exception($"Product with ID {id} was not found") { }
  public class UserAlreadyExistsException(string email) : Exception($"User with email {email} already exists") { }
  public class DatabaseOperationException(Operations operation, Exception inner) : Exception($"Database error during {operation}", inner) { }
  public class UserNotFoundByRefreshException() : Exception("User with such refresh token not found") { }

}