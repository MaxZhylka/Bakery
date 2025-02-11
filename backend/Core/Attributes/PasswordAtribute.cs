using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace backend.Core.Attributes
{


  public class PasswordAttribute : ValidationAttribute
  {
    private const string _pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%#*?&])[A-Za-z\d@$#!%*?&]{8,}$";

    public PasswordAttribute()
    {
      ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.";
    }

    public override bool IsValid(object? value)
    {
      if (value is null)
        return false;

      return value is string password && Regex.IsMatch(password, _pattern);
    }
  }
}