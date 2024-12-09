using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using PRJ4.Repositories;
using PRJ4.DTOs;
using PRJ4.Services;
using PRJ4.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework.Legacy;

namespace PRJ4.Test.ServiceTests
{
    public class FudgifterServiceTests
    {
        private readonly Mock<IFudgifterRepo> _fudgifterRepoMock;
        private readonly Mock<IKategoriRepo> _kategoriRepoMock;
        private readonly Mock<ILogger<FudgifterService>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly FudgifterService _service;

        public FudgifterServiceTests()
        {
            _fudgifterRepoMock = new Mock<IFudgifterRepo>();
            _kategoriRepoMock = new Mock<IKategoriRepo>();
            _loggerMock = new Mock<ILogger<FudgifterService>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Fudgifter, FudgifterResponseDTO>();
                cfg.CreateMap<nyFudgifterDTO, Fudgifter>();
            });
            _mapper = config.CreateMapper();

            _service = new FudgifterService(
                _fudgifterRepoMock.Object,
                _kategoriRepoMock.Object,
                _loggerMock.Object,
                _mapper
            );
        }

        [Test]
        public async Task GetAllByUser_ShouldReturnMappedDTOs_WhenDataExists()
        {
            // Arrange
            var brugerId = "user-1";
            var expenses = new List<Fudgifter>
            {
                new Fudgifter { FudgiftId = 1, Tekst = "Expense1", Pris = 100, BrugerId = brugerId },
                new Fudgifter { FudgiftId = 2, Tekst = "Expense2", Pris = 200, BrugerId = brugerId }
            };
            _fudgifterRepoMock.Setup(r => r.GetAllByUserId(brugerId))
                .ReturnsAsync(expenses);

            // Act
            var result = await _service.GetAllByUser(brugerId);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            result.First().Tekst.Should().Be("Expense1");
            result.First().Pris.Should().Be(100);
        }

        [Test]
        public async Task AddFudgifter_ShouldAddAndReturnDTO_WhenCategoryExists()
        {
            // Arrange
            var brugerId = "user-1";
            var nyFudgifter = new nyFudgifterDTO { Tekst = "Expense", Pris = 300, KategoriNavn = "Groceries" };
            var kategori = new Kategori { KategoriId = 1, KategoriNavn = "Groceries" };
            
            // Set up the added Fudgifter with a valid ID
            var addedFudgifter = new Fudgifter
            {
                FudgiftId = 1, // Set the ID to a non-zero value
                BrugerId = brugerId,
                Tekst = nyFudgifter.Tekst,
                Pris = nyFudgifter.Pris,
                Kategori = kategori,
                KategoriId = kategori.KategoriId,
                Dato = DateTime.Now
            };

            // Set up the response DTO based on the added Fudgifter
            var expectedResponse = new FudgifterResponseDTO
            {
                FudgiftId = addedFudgifter.FudgiftId,
                Pris = addedFudgifter.Pris,
                Tekst = addedFudgifter.Tekst,
                KategoriNavn = addedFudgifter.Kategori.KategoriNavn,
                Dato = addedFudgifter.Dato
            };

            // Mock the repository to return the proper category and handle the add operation
            _kategoriRepoMock.Setup(r => r.SearchByName("Groceries"))
                .ReturnsAsync(kategori);

            _fudgifterRepoMock.Setup(r => r.AddAsync(It.IsAny<Fudgifter>()))
                .ReturnsAsync(addedFudgifter); // Return the Fudgifter with an ID

            _fudgifterRepoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.AddFudgifter(brugerId, nyFudgifter);

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(expectedResponse.Pris, result.Pris);
            ClassicAssert.AreEqual(expectedResponse.Tekst, result.Tekst);

            // Verify interactions with the mocks
            _kategoriRepoMock.Verify(r => r.SearchByName("Groceries"), Times.Once);
            _fudgifterRepoMock.Verify(r => r.AddAsync(It.IsAny<Fudgifter>()), Times.Once);
            _fudgifterRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

           



        [Test]
        public async Task UpdateFudgifter_ShouldThrowUnauthorized_WhenBrugerIdDoesNotMatch()
        {
            // Arrange
            var brugerId = "user-1";
            var fudgifter = new Fudgifter { FudgiftId = 1, BrugerId = "user-2" };
            var updateDTO = new FudgifterUpdateDTO { Pris = 500 };

            _fudgifterRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(fudgifter);

            // Act
            var act = async () => await _service.UpdateFudgifter(brugerId, 1, updateDTO);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Unauthorized.");
        }

        [Test]
        public async Task DeleteFudgifter_ShouldDelete_WhenBrugerIdMatches()
        {
            // Arrange
            var brugerId = "user-1";
            var fudgifter = new Fudgifter { FudgiftId = 1, BrugerId = brugerId };

            _fudgifterRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(fudgifter);

            // Act
            await _service.DeleteFudgifter(brugerId, 1);

            // Assert
            _fudgifterRepoMock.Verify(r => r.Delete(1), Times.Once);
            _fudgifterRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteFudgifter_ShouldThrowUnauthorized_WhenBrugerIdDoesNotMatch()
        {
            // Arrange
            var brugerId = "user-1";
            var fudgifter = new Fudgifter { FudgiftId = 1, BrugerId = "user-2" };

            _fudgifterRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(fudgifter);

            // Act
            var act = async () => await _service.DeleteFudgifter(brugerId, 1);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Unauthorized.");
        }
    }
}
