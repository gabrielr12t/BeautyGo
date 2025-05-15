using BeautyGo.Application.Businesses.Commands.SendProfessionalRequest;
using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Users;
using BeautyGo.Domain.Core.Errors;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Entities.Persons;
using BeautyGo.Domain.Entities.Professionals;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories.Bases;
using FluentAssertions;
using Moq;

namespace BeautyGo.Application.Tests;

/// <summary>
/// DOC
/// 
/// Título: Enviar convite para profissional vincular-se à minha loja
/// Como proprietário de uma loja(business owner)
/// Quero enviar um convite para um usuário se tornar profissional na minha loja
/// Para que ele possa aceitar e começar a atender clientes dentro do sistema
///
/// - Critérios de Aceitação:
/// 
/// - O usuário que envia o convite deve ser um dono de loja válido e autorizado;
///
/// - A loja deve estar ativa e não excluída;
///
/// - O usuário convidado deve existir no sistema;
///
/// - Só pode existir um convite ativo por profissional por loja;
///
/// - Se já houver um convite anterior não expirado, o sistema deve retornar um erro;
///
/// - Um novo convite é criado apenas quando todas as regras forem satisfeitas;
///
/// - O convite deve ser persistido com sucesso e estar associado ao profissional e à loja.
///
/// </summary>

public class SendProfessionalRequestCommandHandlerTests
{
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<IEFBaseRepository<User>> _userRepositoryMock = new();
    private readonly Mock<IEFBaseRepository<Business>> _businessRepositoryMock = new();
    private readonly Mock<IEFBaseRepository<ProfessionalRequest>> _professionalRequestRepositoryMock = new();
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private SendProfessionalRequestCommandHandler CreateHandler()
    {
        return new SendProfessionalRequestCommandHandler(
            _userServiceMock.Object,
            _authServiceMock.Object,
            _unitOfWorkMock.Object,
            _userRepositoryMock.Object,
            _businessRepositoryMock.Object,
            _professionalRequestRepositoryMock.Object);
    }

    [Fact]
    public async Task Should_Fail_When_User_IsNot_Owner()
    {
        // Arrange
        var command = new SendProfessionalRequestCommand(Guid.NewGuid(), Guid.NewGuid());
        _userServiceMock.Setup(s => s.AuthorizeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DomainErrors.General.UnauthorizedUser);
    }

    [Fact]
    public async Task Should_Fail_When_Business_Does_Not_Exist()
    {
        // Arrange
        var command = new SendProfessionalRequestCommand(Guid.NewGuid(), Guid.NewGuid());

        _userServiceMock.Setup(s => s.AuthorizeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _userRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Specification<User>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _businessRepositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Specification<Business>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Business)null);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Business.BusinessNotFound");
    }

    [Fact]
    public async Task Should_Fail_When_Professional_Request_Already_Exists_And_Not_Expired()
    {
        // Arrange
        var command = new SendProfessionalRequestCommand(Guid.NewGuid(), Guid.NewGuid());

        var ownerMock = new Mock<BusinessOwner>(MockBehavior.Default);
        var owner = ownerMock.Object;

        var business = new Business() { Id = Guid.NewGuid(), Name = "Barbearia", IsActive = true, Owner = owner };

        var existingRequest = new ProfessionalRequest(); // assume que esse método está dentro do seu domínio

        _userServiceMock.Setup(s => s.AuthorizeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _userRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Specification<User>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _businessRepositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Specification<Business>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(business);

        _authServiceMock.Setup(s => s.GetCurrentUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(owner); // é o dono da loja aqui

        _professionalRequestRepositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Specification<ProfessionalRequest>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingRequest);

        // simula que ainda está válido
        existingRequest.ExpireAt = DateTime.Now.AddHours(1);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DomainErrors.ProfessionalRequest.ProfessionalRequestAlreadyExists);
    }
     
    [Fact]
    public async Task Should_Succeed_When_Valid_And_Persist_ProfessionalRequest()
    {
        // Arrange
        var command = new SendProfessionalRequestCommand(Guid.NewGuid(), Guid.NewGuid());

        var ownerMock = new Mock<BusinessOwner>(MockBehavior.Default);
        var owner = ownerMock.Object;

        // Cria um Business real com o dono como o "user"
        var business = new Business() { Id = command.BusinessId, Name = "Loja Legal", Owner = owner, IsActive = true }; // supondo que o construtor aceite o dono

        _userServiceMock.Setup(s => s.AuthorizeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _userRepositoryMock.Setup(r => r.ExistAsync(It.IsAny<Specification<User>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _businessRepositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Specification<Business>>(), false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(business);

        _businessRepositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Specification<Business>>(), true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(business);

        _authServiceMock.Setup(s => s.GetCurrentUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(owner);

        _professionalRequestRepositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Specification<ProfessionalRequest>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProfessionalRequest)null);

        _userRepositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Specification<User>>(), true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(owner);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _businessRepositoryMock.Verify(r => r.UpdateAsync(business), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}



