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
using FluentAssertions;
using PRJ4.Data;
using PRJ4.Test.Setup;



namespace PRJ4.Test.SystemTest
{
    public class FudgifterSystemTest : TestBase, IDisposable
    {
        private readonly Mock<IFudgifterService> _fudgifterServiceMock;
        private readonly Mock<ILogger<FudgifterController>> _loggerMock;
        private readonly FudgifterController _controller;

        private ApplicationDbContext _context;
        public FudgifterSystemTest()
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
        [SetUp] // This runs before each test
        public void Setup()
        {
            _context = CreateContext(); // Initialize the context for each test
        }

        [TearDown] // This runs after each test
        public void Dispose()
        {
            _context?.Dispose(); // Dispose of the context properly
        }

        [Test]
        public async Task AddFudgifter_ReturnsOk_WhithNewDataExists()
        {
            // Arrange
            const string userId = "user-1";
            SetupUserContext(userId);

            var nyFudgifterDTO = new nyFudgifterDTO { Pris = 5000, Tekst = "Monthly rent", Dato = DateTime.Now };

            // Act
            var result = await _controller.Add(nyFudgifterDTO);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();

            var createdAtResult = result.Result as CreatedAtActionResult;
            createdAtResult.Should().NotBeNull();

            // Verify the response data
            var responseDTO = createdAtResult.Value as FudgifterResponseDTO;
            responseDTO.Should().NotBeNull();
            responseDTO.Pris.Should().Be(nyFudgifterDTO.Pris);
            responseDTO.Tekst.Should().Be(nyFudgifterDTO.Tekst);
        }
    }
}