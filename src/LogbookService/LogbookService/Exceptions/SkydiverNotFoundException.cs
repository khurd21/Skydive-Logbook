namespace LogbookService.Exceptions;

public class SkydiverNotFoundException : Exception
{
    public SkydiverNotFoundException(in string email)
        : base($"Skydiver {email} not found")
    {
    }

    public SkydiverNotFoundException(in string email, in Exception innerException)
        : base($"Skydiver {email} not found", innerException)
    {
    }
}