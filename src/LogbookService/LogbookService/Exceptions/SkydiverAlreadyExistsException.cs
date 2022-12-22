namespace LogbookService.Exceptions;

public class SkydiverAlreadyExistsException : Exception
{
    public SkydiverAlreadyExistsException(in string email)
        : base($"Skydiver {email} already exists")
    {
    }

    public SkydiverAlreadyExistsException(in string email, in Exception innerException)
        : base($"Skydiver {email} already exists", innerException)
    {
    }
}