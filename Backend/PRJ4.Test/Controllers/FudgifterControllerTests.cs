using Xunit;
using Moq;
using PRJ4.Controllers;
using PRJ4.Services;
using PRJ4.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using NUnit.Framework.Legacy;

public class FudgifterControllerTests
{
    private readonly Mock<IFudgifterService> _mockFudgifterService;
    private readonly Mock<ILogger<FudgifterController>> _mockLogger;
    private readonly FudgifterController _controller;

    public FudgifterControllerTests()
    {
        _mockFudgifterService = new Mock<IFudgifterService>();
        _mockLogger = new Mock<ILogger<FudgifterController>>();

        _controller = new FudgifterController(_mockFudgifterService.Object, _mockLogger.Object);

        // Simulate authentication context
        var mockUserId = "user-1";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", mockUserId)
                }))
            }
        };
    }

    [Fact]
    public async Task GetAllByUser_ReturnsOk_WithExpectedData()
    {
        // Arrange
        var expectedFudgifter = new List<FudgifterResponseDTO>
        {
            new FudgifterResponseDTO { FudgiftId = 1, Tekst = "Rent", Pris = 5000 },
            new FudgifterResponseDTO { FudgiftId = 2, Tekst = "Electricity", Pris = 1500 }
        };

        _mockFudgifterService
            .Setup(service => service.GetAllByUser(It.IsAny<string>()))
            .ReturnsAsync(expectedFudgifter);

        // Act
        var result = await _controller.GetAllByUser();

        // Assert
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result.Result);
        var actualData = Xunit.Assert.IsType<List<FudgifterResponseDTO>>(okResult.Value);

        Xunit.Assert.Equal(expectedFudgifter.Count, actualData.Count);
        Xunit.Assert.Equal(expectedFudgifter[0].Tekst, actualData[0].Tekst);
    }
}
