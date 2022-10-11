namespace LogbookService.Exceptions;

public class JumpNotFoundException : Exception
{
    public JumpNotFoundException(in int uspaMembershipNumber, in int jumpNumber)
        : base($"Jump {jumpNumber} for skydiver {uspaMembershipNumber} not found")
    {
    }

    public JumpNotFoundException(in int uspaMembershipNumber, in int jumpNumber, in Exception innerException)
        : base($"Jump {jumpNumber} for skydiver {uspaMembershipNumber} not found", innerException)
    {
    }
}