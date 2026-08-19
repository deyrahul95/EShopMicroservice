namespace BuildingBlock.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message: message)
    {

    }

    public NotFoundException(string name, string key) : base(message: $"Entity '{name}' ({key}) was not found.")
    {

    }
}
