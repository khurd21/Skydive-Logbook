namespace LogbookService.Exceptions;

public class JumpNotFoundException : Exception
{
    public JumpNotFoundException(in string id, in int jumpNumber)
        : base($"Jump {jumpNumber} for skydiver {id} not found")
    {
    }

    public JumpNotFoundException(in string id, in int jumpNumber, in Exception innerException)
        : base($"Jump {jumpNumber} for skydiver {id} not found", innerException)
    {
    }
}