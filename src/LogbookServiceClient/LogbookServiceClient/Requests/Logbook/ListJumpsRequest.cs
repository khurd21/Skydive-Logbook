using System.ComponentModel.DataAnnotations;
using FoolProof.Core;

namespace Logbook.Requests.Logbook;

public class ListJumpsRequest
{
    [Range(1, int.MaxValue, ErrorMessage=$"{nameof(this.From)} must be greater than 0")]
    public int From { get; init; } = 1;

    [GreaterThan(nameof(this.From), ErrorMessage=$"{nameof(this.To)} must be greater than {nameof(this.From)}")]
    public int To { get; init; } = int.MaxValue;
}