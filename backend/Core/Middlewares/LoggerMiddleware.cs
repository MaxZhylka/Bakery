using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using backend.Core.Enums;
using backend.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace backend.Core.Middlewares
{
  public class LoggerMiddleware(RequestDelegate next)
  {
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context, ILoggerService loggerService)
    {
      var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

      if (userIdClaim == null)
      {
        await _next(context);
        return;
      }

      if (!Guid.TryParse(userIdClaim.Value, out var userId))
      {
        await _next(context);
        return;
      }

      var operation = GetOperationFromEndpoint(context);

      if (operation != null)
      {
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        var ip = forwardedFor?.Split(',').First().Trim() ?? context.Connection.RemoteIpAddress?.ToString();
        var details = $"Method: {context.Request.Method}, IP: {ip}";
        await loggerService.LogAction(userId, operation.Value, details);
      }

      await _next(context);
    }

    private static Operations? GetOperationFromEndpoint(HttpContext context)
    {
      var endpoint = context.GetEndpoint();
      if (endpoint == null)
        return null;

      var controllerActionDescriptor = endpoint.Metadata.OfType<ControllerActionDescriptor>().FirstOrDefault();

      if (controllerActionDescriptor == null)
        return null;

      var actionName = controllerActionDescriptor.ActionName;
      var controllerName = controllerActionDescriptor.ControllerName;

      return (controllerName, actionName) switch
      {
        ("Users", "CreateUser") => Operations.CreateUser,
        ("Users", "UpdateUser") => Operations.UpdateUser,
        ("Users", "DeleteUser") => Operations.DeleteUser,

        ("Orders", "CreateOrder") => Operations.CreateOrder,
        ("Orders", "DeleteOrder") => Operations.DeleteOrder,
        ("Orders", "UpdateOrder") => Operations.UpdateOrder,

        ("Products", "CreateProduct") => Operations.CreateProduct,
        ("Products", "DeleteProduct") => Operations.DeleteProduct,
        ("Products", "UpdateProduct") => Operations.UpdateProduct,

        ("Logger", "GetLogs") => Operations.GetAllLogs,
        ("Logger", "GetLogsByUserId") => Operations.GetLogsByUserID,
        ("Logger", "GetLogById") => Operations.GetLogById,

        ("Auth", "Logout") => Operations.Logout,
        _ => null
      };
    }
  }
}