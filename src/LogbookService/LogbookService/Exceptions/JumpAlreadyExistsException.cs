namespace LogbookService.Exceptions;

public class JumpAlreadyExistsException : Exception
{
    public JumpAlreadyExistsException(in string id, in int jumpNumber)
        : base($"Jump {jumpNumber} for skydiver {id} already exists")
    {
    }

    public JumpAlreadyExistsException(in string id, in int jumpNumber, in Exception innerException)
        : base($"Jump {jumpNumber} for skydiver {id} already exists", innerException)
    {
    }
}