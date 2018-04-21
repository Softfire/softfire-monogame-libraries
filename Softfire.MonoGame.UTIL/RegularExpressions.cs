using System.Text.RegularExpressions;

namespace Softfire.MonoGame.UTIL
{
    public static class RegularExpressions
    {
        public static Regex EmailAddress { get; } = new Regex(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$");

        public static Regex PhoneNumber { get; } = new Regex(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$");
    }
}