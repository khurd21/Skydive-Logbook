using LogbookService.Records;

namespace LogbookServiceClient.Test.TestCaseSources;

public static class LoggedJumpTestCaseSources
{
    public static IEnumerable<IEnumerable<LoggedJump>> ListJumps_Valid
    {
        get  
        {
            yield return new List<LoggedJump>();
            yield return new List<LoggedJump>
            {
                new LoggedJump()
                {
                    JumpNumber = 1,
                    Id = "123456",
                    Date = new DateTime(2021, 1, 1),
                    Aircraft = "Cessna 172",
                }
            };
            yield return new List<LoggedJump>
            {
                new LoggedJump()
                {
                    JumpNumber = 1,
                    Id = "123456",
                    Date = new DateTime(2021, 1, 1),
                    Aircraft = "Cessna 172",
                },
                new LoggedJump()
                {
                    JumpNumber = 2,
                    Id = "123456",
                    Date = new DateTime(2021, 1, 2),
                    Aircraft = "Cessna 172",
                    JumpCategory = LogbookService.Records.Enums.JumpCategory.FREEFLY,
                    Description = "Test jump",
                }
            };
        }
    }

    public static IEnumerable<LoggedJump> LogJump_Valid
    {
        get
        {
            yield return new LoggedJump()
            {
                JumpNumber = 1,
                Id = "123456",
                Date = new DateTime(2021, 1, 1),
                Aircraft = "Cessna 172",
            };
            yield return new LoggedJump()
            {
                JumpNumber = 2,
                Id = "123456",
                Date = new DateTime(2021, 1, 2),
                Aircraft = "Cessna 172",
                JumpCategory = LogbookService.Records.Enums.JumpCategory.FREEFLY,
                Description = "Test jump",
            };
        }
    }
}