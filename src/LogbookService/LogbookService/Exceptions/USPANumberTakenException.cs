namespace LogbookService.Exceptions;

public class USPANumberTakenException : Exception
{
    public USPANumberTakenException(in int uspaNumber)
        : base($"USPA number {uspaNumber} is already taken")
    {
    }

    public USPANumberTakenException(in int uspaNumber, in Exception innerException)
        : base($"USPA number {uspaNumber} is already taken", innerException)
    {
    }
}