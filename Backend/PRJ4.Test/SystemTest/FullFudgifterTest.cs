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
using PRJ4.Repositories;
using PRJ4.Models;
using PRJ4.Mappings;
using AutoMapper;

namespace PRJ4.Test.SystemTest
{
    public class FudgifterSystemTest : TestBase, IDisposable
    {
        private readonly IFudgifterService _fudgifterService;
        private readonly Mock<ILogger<FudgifterController>> _loggerControllerMock;
        private readonly Mock<ILogger<FudgifterService>> _loggerServiceMock;
        private readonly IMapper _mapper;
        private readonly FudgifterController _controller;
        private readonly IKategoriRepo _kategoriRepo;
        private readonly IFudgifterRepo _fudgifterRepo;
        private ApplicationDbContext _context;

        public FudgifterSystemTest()
        {
            _loggerControllerMock = new Mock<ILogger<FudgifterController>>();
            _loggerServiceMock = new Mock<ILogger<FudgifterService>>();

            var fudgifterProfile = new FudgifterProfile();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(fudgifterProfile));
            _mapper = new Mapper(mapperConfig);

            _context = CreateContext();
            _kategoriRepo = new KategoriRepo(_context);
            _fudgifterRepo = new FudgifterRepo(_context);

            _fudgifterService = new FudgifterService(
                _fudgifterRepo,
                _kategoriRepo,
                _loggerServiceMock.Object,
                _mapper
            );

            _controller = new FudgifterController(_fudgifterService, _loggerControllerMock.Object);
        }

        private void SetupUserContext(string userId)
        {
            var claims = new List<Claim> 
            { 
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId) 
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };
            _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
        }

        [SetUp]
        public void Setup() => _context = CreateContext();

        [TearDown]
        public void Dispose() => _context?.Dispose();

        // Controller Tests

        [Test]
        public async Task GetAllByUser_ReturnsOk_WhenDataExists()
        {
            SetupUserContext("user-1");

            var kategori = new Kategori { KategoriId = 1, KategoriNavn = "Utilities" };
            _context.Kategorier.Add(kategori);
            _context.Fudgifters.AddRange(
                new Fudgifter { Tekst = "Expense1", Pris = 100, BrugerId = "user-1", Kategori = kategori },
                new Fudgifter { Tekst = "Expense2", Pris = 200, BrugerId = "user-1", Kategori = kategori }
            );
            await _context.SaveChangesAsync();

            var result = await _controller.GetAllByUser();

            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var returnedList = okResult?.Value as List<FudgifterResponseDTO>;

            returnedList.Should().NotBeNull();
            returnedList.Count.Should().Be(2);
        }

        [Test]
        public async Task Add_ReturnsCreatedAtAction_WhenSuccess()
        {
            SetupUserContext("user-1");

            var nyFudgifterDTO = new nyFudgifterDTO 
            { 
                Pris = 500, 
                Tekst = "New Expense", 
                KategoriNavn = "Groceries" 
            };

            var result = await _controller.Add(nyFudgifterDTO);

            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdAtResult = result.Result as CreatedAtActionResult;

            var responseDTO = createdAtResult?.Value as FudgifterResponseDTO;
            responseDTO.Should().NotBeNull();
            responseDTO.Tekst.Should().Be(nyFudgifterDTO.Tekst);
            responseDTO.Pris.Should().Be(nyFudgifterDTO.Pris);
        }

    }
}
