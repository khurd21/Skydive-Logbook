using System.Text.Json.Serialization;

namespace LogbookService.Records.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
[Flags]
public enum JumpCategory
{
    BELLY = 0,
    FREEFLY = 1,
    WINGSUIT = 2,
    HIGHPULL = 3,
    TANDEM = 4,
    AFF = 5,
    CRW = 6,
    XRW = 7,
};