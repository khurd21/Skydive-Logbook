namespace Logbook.Requests.Logbook;

public class ListJumpsRequest
{
    public int USPAMembershipNumber { get; init; }
    public int From { get; init; }
    public int To { get; init; }
}