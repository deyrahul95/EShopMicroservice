namespace BuildingBlock.Exceptions;

public class BadRequestException : Exception
{
    public string? Details { get; }

    public BadRequestException(string message) : base(message: message)
    {

    }

    public BadRequestException(string message, string details) : base(message: message)
    {
        Details = details;
    }
}
