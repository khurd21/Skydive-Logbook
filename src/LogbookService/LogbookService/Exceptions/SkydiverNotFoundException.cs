namespace LogbookService.Exceptions;

public class SkydiverNotFoundException : Exception
{
    public SkydiverNotFoundException(in int uspaMembershipNumber)
        : base($"Skydiver {uspaMembershipNumber} not found")
    {
    }

    public SkydiverNotFoundException(in int uspaMembershipNumber, in Exception innerException)
        : base($"Skydiver {uspaMembershipNumber} not found", innerException)
    {
    }
}