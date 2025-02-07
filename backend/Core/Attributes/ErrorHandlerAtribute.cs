using Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Core.Attributes
{
  public class ErrorHandlerAttribute : ExceptionFilterAttribute
  {
    public override void OnException(ExceptionContext context)
    {
      IActionResult result = context.Exception switch
      {
        UserNotFoundException ex => new NotFoundObjectResult(new
        {
          error = ex.Message
        }),

        UserNotFoundByRefreshException ex => new NotFoundObjectResult(new
        {
          error = ex.Message
        }),

        UserAlreadyExistsException ex => new ObjectResult(new
        {
          error = ex.Message
        })
        {
          StatusCode = StatusCodes.Status400BadRequest
        },

        DatabaseOperationException ex => new ObjectResult(new
        {
          error = ex.Message
        })
        {
          StatusCode = StatusCodes.Status500InternalServerError
        },

        _ => new ObjectResult(new { error = "An unexpected error occurred" })
        {
          StatusCode = StatusCodes.Status500InternalServerError
        }
      };

      context.Result = result;
      context.ExceptionHandled = true;
    }
  }
}