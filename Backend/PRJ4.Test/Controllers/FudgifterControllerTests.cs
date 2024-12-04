using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PRJ4.Controllers;
using PRJ4.DTOs;
using PRJ4.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FluentAssertions;  // Add this import

public class FudgifterControllerTests
{
    private readonly Mock<IFudgifterService> _fudgifterServiceMock;
    private readonly Mock<ILogger<FudgifterController>> _loggerMock;
    private readonly FudgifterController _controller;

    public FudgifterControllerTests()
    {
        _fudgifterServiceMock = new Mock<IFudgifterService>();
        _loggerMock = new Mock<ILogger<FudgifterController>>();
        _controller = new FudgifterController(_fudgifterServiceMock.Object, _loggerMock.Object);
    }

    private void SetupUserContext(string userId)
    {
        var claims = new List<Claim> { new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
    }

    [Test]
    public async Task GetAllByUser_ReturnsOk_WhenDataExists()
    {
        // Arrange
        SetupUserContext("user-1");
        var fudgifterList = new List<FudgifterResponseDTO>
        {
            new FudgifterResponseDTO { FudgiftId = 1, Tekst = "Expense1", Pris = 100 },
            new FudgifterResponseDTO { FudgiftId = 2, Tekst = "Expense2", Pris = 200 }
        };
        _fudgifterServiceMock.Setup(s => s.GetAllByUser("user-1"))
            .ReturnsAsync(fudgifterList);

        // Act
        var result = await _controller.GetAllByUser();

        // Assert
        var okResult = result.Result as OkObjectResult; // Safely cast to OkObjectResult
        Xunit.Assert.NotNull(okResult); // Ensure the result is not null
        var returnedList = okResult.Value as List<FudgifterResponseDTO>; // Cast the Value to the expected type
        Xunit.Assert.NotNull(returnedList); // Ensure the Value is not null
        Xunit.Assert.Equal(fudgifterList, returnedList); // Compare the lists
    }



    [Test]
    public async Task GetAllByUser_ReturnsBadRequest_OnException()
    {
        // Arrange
        SetupUserContext("user-1");
        _fudgifterServiceMock.Setup(s => s.GetAllByUser("user-1"))
            .ThrowsAsync(new System.Exception("Database error"));

        // Act
        var result = await _controller.GetAllByUser();

        // Assert
        var okResult = result.Result as BadRequestObjectResult;
        Xunit.Assert.NotNull(okResult);
        Xunit.Assert.Equal(okResult.Value, "Database error");
        
    }



    [Test]
    public async Task Add_ReturnsCreatedAtAction_WhenSuccess()
    {
        // Arrange
        SetupUserContext("user-1");
        var input = new nyFudgifterDTO { Tekst = "New Expense", Pris = 500, KategoriId = 1 };
        var output = new FudgifterResponseDTO { FudgiftId = 10, Tekst = "New Expense", Pris = 500 };

        _fudgifterServiceMock.Setup(s => s.AddFudgifter("user-1", input))
            .ReturnsAsync(output);

        // Act
        var result = await _controller.Add(input);

        // Assert
        var createdAtAct = result.Result as CreatedAtActionResult;
        var returnedObject = createdAtAct.Value as FudgifterResponseDTO;
        Xunit.Assert.NotNull(createdAtAct);
        Xunit.Assert.Equal(output, returnedObject);
    }


    [Test]
    public async Task Add_ReturnsBadRequest_OnException()
    {
        // Arrange
        SetupUserContext("user-1");
        var input = new nyFudgifterDTO { Tekst = "New Expense", Pris = 500, KategoriId = 1 };
        _fudgifterServiceMock.Setup(s => s.AddFudgifter("user-1", input))
            .ThrowsAsync(new System.Exception("Service error"));

        // Act
        var result = await _controller.Add(input);

        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        Xunit.Assert.NotNull(badRequestResult);
        Xunit.Assert.Equal(badRequestResult.Value, "Service error");
    }


    [Test]
    public async Task Update_ReturnsNoContent_WhenSuccess()
    {
        // Arrange
        SetupUserContext("user-1");
        var updateDTO = new FudgifterUpdateDTO { Tekst = "Updated Expense", Pris = 300 };

        // Act
        var result = await _controller.Update(1, updateDTO);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _fudgifterServiceMock.Verify(s => s.UpdateFudgifter("user-1", 1, updateDTO), Times.Once);
    }

    [Test]
    public async Task Update_ReturnsBadRequest_OnException()
    {
        // Arrange
        SetupUserContext("user-1");
        var updateDTO = new FudgifterUpdateDTO { Tekst = "Updated Expense", Pris = 300 };

        _fudgifterServiceMock.Setup(s => s.UpdateFudgifter("user-1", 1, updateDTO))
            .ThrowsAsync(new System.Exception("Update error"));

        // Act
        var result = await _controller.Update(1, updateDTO);

        // Assert
        
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult?.Value.Should().Be("Update error");
    }

    [Test]
    public async Task Delete_ReturnsNoContent_WhenSuccess()
    {
        // Arrange
        SetupUserContext("user-1");
        _fudgifterServiceMock.Setup(s => s.DeleteFudgifter("user-1", 1))
            .Returns(Task.CompletedTask); // Simulate success

        // Act
        var result = await _controller.Delete(1);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _fudgifterServiceMock.Verify(s => s.DeleteFudgifter("user-1", 1), Times.Once);
    }


    [Test]
    public async Task Delete_ReturnsBadRequest_OnException()
    {
        // Arrange
        SetupUserContext("user-1");

        _fudgifterServiceMock.Setup(s => s.DeleteFudgifter("user-1", 1))
            .ThrowsAsync(new System.Exception("Delete error"));

        // Act
        var result = await _controller.Delete(1);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult?.Value.Should().Be("Delete error");
    }
}
