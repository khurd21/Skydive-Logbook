using Logbook.Requests.Logbook;
using LogbookService.Exceptions;
using LogbookService.Records;
using Microsoft.AspNetCore.Http;

namespace LogbookServiceClient.Test.TestCaseSources;

public static class LogbookControllerTestCases
{
    public static readonly object[] ListJumpsExceptionCases =
    {
        new object[]
        {
            123456,
            "Failed to list jumps",
            StatusCodes.Status500InternalServerError,
            new Exception("Random exception")
        },
        new object[]
        {
            123456,
            "Skydiver with USPA membership number 123456 not found",
            StatusCodes.Status404NotFound,
            new SkydiverNotFoundException(123456)
        },
    };

    public static readonly object[] EditJumpExceptionCases =
    {
        new object[]
        {
            new EditJumpRequest()
            {
                Jump = new LoggedJump()
                {
                    USPAMembershipNumber = 123456,
                    JumpNumber = 1,
                },
            },
            "Failed to edit jump",
            StatusCodes.Status500InternalServerError,
            new Exception("Random exception"),
        },
        new object[]
        {
            new EditJumpRequest()
            {
                Jump = new LoggedJump()
                {
                    USPAMembershipNumber = 123456,
                    JumpNumber = 1,
                },
            },
            "Skydiver with USPA membership number 123456 not found",
            StatusCodes.Status404NotFound,
            new SkydiverNotFoundException(123456),
        },
        new object[]
        {
            new EditJumpRequest()
            {
                Jump = new LoggedJump()
                {
                    USPAMembershipNumber = 123456,
                    JumpNumber = 1,
                },
            },
            "Jump with number 1 not found",
            StatusCodes.Status404NotFound,
            new JumpNotFoundException(123456, 1),
        },
    };

    public static readonly object[] DeleteJumpExceptionCases =
    {
        new object[]
        {
            new DeleteJumpRequest()
            {
                USPAMembershipNumber = 123456,
                JumpNumber = 1,
            },
            "Failed to delete jump",
            StatusCodes.Status500InternalServerError,
            new Exception("Random exception"),
        },
        new object[]
        {
            new DeleteJumpRequest()
            {
                USPAMembershipNumber = 123456,
                JumpNumber = 1,
            },
            "Jump with number 1 from member 123456 not found",
            StatusCodes.Status404NotFound,
            new JumpNotFoundException(123456, 1),
        },
    };
}