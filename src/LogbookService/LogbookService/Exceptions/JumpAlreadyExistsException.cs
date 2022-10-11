namespace LogbookService.Exceptions;

public class JumpAlreadyExistsException : Exception
{
    public JumpAlreadyExistsException(in int uspaMembershipNumber, in int jumpNumber)
        : base($"Jump {jumpNumber} for skydiver {uspaMembershipNumber} already exists")
    {
    }

    public JumpAlreadyExistsException(in int uspaMembershipNumber, in int jumpNumber, in Exception innerException)
        : base($"Jump {jumpNumber} for skydiver {uspaMembershipNumber} already exists", innerException)
    {
    }
}