namespace LogbookService.Exceptions;

public class LogbookServiceException : Exception
{
    public LogbookServiceException(string message) : base(message)
    {
    }

    public LogbookServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}