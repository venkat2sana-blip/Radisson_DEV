namespace Radisson_RHG.Exceptions;

public class NotFoundException : AppException
{
    public NotFoundException() { }
    public NotFoundException(string message) : base(message) { }
}
