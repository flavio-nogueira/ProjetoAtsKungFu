using ApiAtsKungFu.Application.DTOs;
using ApiAtsKungFu.Application.Services;
using ApiAtsKungFu.Domain.Entities;
using ApiAtsKungFu.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ApiAtsKungFu.Tests.Services
{
    public class EscolaKungFuServiceTests
    {
        private readonly Mock<IEscolaKungFuRepository> _repositoryMock;
        private readonly EscolaKungFuService _service;

        public EscolaKungFuServiceTests()
        {
            _repositoryMock = new Mock<IEscolaKungFuRepository>();
            _service = new EscolaKungFuService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarEscola_QuandoExistir()
        {
            // Arrange
            var escola = EscolaKungFu.CriarMatriz(
                "12345678000190",
                "Academia de Kung Fu LTDA",
                "Academia Master",
                "Rua das Artes Marciais",
                "100",
                "Centro",
                "São Paulo",
                "SP",
                "01234567"
            );

            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(escola);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.CNPJ.Should().Be("12345678000190");
            result.RazaoSocial.Should().Be("Academia de Kung Fu LTDA");
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExistir()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((EscolaKungFu?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_DeveCriarMatriz_ComSucesso()
        {
            // Arrange
            var createDto = new CreateEscolaKungFuDto
            {
                Tipo = "Matriz",
                CNPJ = "12345678000190",
                RazaoSocial = "Academia de Kung Fu LTDA",
                NomeFantasia = "Academia Master",
                Logradouro = "Rua das Artes Marciais",
                Numero = "100",
                Bairro = "Centro",
                Cidade = "São Paulo",
                UF = "SP",
                CEP = "01234567"
            };

            _repositoryMock.Setup(r => r.CNPJExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<EscolaKungFu>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.CNPJ.Should().Be("12345678000190");
            result.EMatriz.Should().BeTrue();
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<EscolaKungFu>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_DeveLancarExcecao_QuandoCNPJJaExistir()
        {
            // Arrange
            var createDto = new CreateEscolaKungFuDto
            {
                Tipo = "Matriz",
                CNPJ = "12345678000190",
                RazaoSocial = "Academia de Kung Fu LTDA",
                Logradouro = "Rua teste",
                Numero = "100",
                Bairro = "Centro",
                Cidade = "São Paulo",
                UF = "SP",
                CEP = "01234567"
            };

            _repositoryMock.Setup(r => r.CNPJExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _service.CreateAsync(createDto));
        }

        [Fact]
        public async Task CreateAsync_DeveCriarFilial_ComSucesso()
        {
            // Arrange
            var createDto = new CreateEscolaKungFuDto
            {
                Tipo = "Filial",
                CNPJ = "12345678000271",
                RazaoSocial = "Academia de Kung Fu LTDA",
                NomeFantasia = "Academia Filial",
                IdEmpresaMatriz = 1,
                Logradouro = "Rua das Artes Marciais",
                Numero = "200",
                Bairro = "Centro",
                Cidade = "São Paulo",
                UF = "SP",
                CEP = "01234567"
            };

            _repositoryMock.Setup(r => r.CNPJExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            _repositoryMock.Setup(r => r.ExistsAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<EscolaKungFu>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.CNPJ.Should().Be("12345678000271");
            result.EMatriz.Should().BeFalse();
            result.IdEmpresaMatriz.Should().Be(1);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<EscolaKungFu>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_DeveLancarExcecao_QuandoFilialSemMatriz()
        {
            // Arrange
            var createDto = new CreateEscolaKungFuDto
            {
                Tipo = "Filial",
                CNPJ = "12345678000271",
                RazaoSocial = "Academia de Kung Fu LTDA",
                Logradouro = "Rua teste",
                Numero = "100",
                Bairro = "Centro",
                Cidade = "São Paulo",
                UF = "SP",
                CEP = "01234567"
                // IdEmpresaMatriz não fornecido
            };

            _repositoryMock.Setup(r => r.CNPJExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _service.CreateAsync(createDto));
        }

        [Fact]
        public async Task UpdateAsync_DeveAtualizarEscola_ComSucesso()
        {
            // Arrange
            var escola = EscolaKungFu.CriarMatriz(
                "12345678000190",
                "Academia Original",
                "Nome Original",
                "Rua Original",
                "100",
                "Bairro",
                "Cidade",
                "SP",
                "01234567"
            );

            var updateDto = new UpdateEscolaKungFuDto
            {
                RazaoSocial = "Academia Atualizada",
                Logradouro = "Rua Nova",
                Numero = "200",
                Bairro = "Bairro Novo",
                Cidade = "São Paulo",
                UF = "SP",
                CEP = "01234567"
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(escola);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<EscolaKungFu>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync(1, updateDto);

            // Assert
            result.Should().NotBeNull();
            result.RazaoSocial.Should().Be("Academia Atualizada");
            result.Logradouro.Should().Be("Rua Nova");
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<EscolaKungFu>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_DeveLancarExcecao_QuandoEscolaNaoExistir()
        {
            // Arrange
            var updateDto = new UpdateEscolaKungFuDto
            {
                RazaoSocial = "Academia Atualizada",
                Logradouro = "Rua Nova",
                Numero = "200",
                Bairro = "Bairro Novo",
                Cidade = "São Paulo",
                UF = "SP",
                CEP = "01234567"
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((EscolaKungFu?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _service.UpdateAsync(999, updateDto));
        }

        [Fact]
        public async Task DeleteAsync_DeveDesativarEscola_ComSucesso()
        {
            // Arrange
            var escola = EscolaKungFu.CriarMatriz(
                "12345678000190",
                "Academia de Kung Fu LTDA",
                "Academia Master",
                "Rua teste",
                "100",
                "Centro",
                "São Paulo",
                "SP",
                "01234567"
            );

            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(escola);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<EscolaKungFu>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();
            escola.Ativo.Should().BeFalse();
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<EscolaKungFu>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeveRetornarFalse_QuandoEscolaNaoExistir()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((EscolaKungFu?)null);

            // Act
            var result = await _service.DeleteAsync(999);

            // Assert
            result.Should().BeFalse();
        }
    }
}
