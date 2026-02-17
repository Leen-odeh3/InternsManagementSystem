namespace IMS.Core.Exceptions;
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
    public BadRequestException(IEnumerable<string> errors)
     : base(string.Join(", ", errors))
    {
    }
}