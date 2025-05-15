using BeautyGo.Application.Core.Abstractions.Authentication;
using BeautyGo.Application.Core.Abstractions.Messaging;
using BeautyGo.Domain.Core.Primitives.Results;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories.Bases;

namespace BeautyGo.Application.Professionals.CreateProfessional;

public class CreateProfessionalCommandHandler : ICommandHandler<CreateProfessionalCommand, Result>
{
    //Criar validator
    //Apenas dono da business pode cadastrar um profissional
    //lançar erro se profissional existir e estiver cadastrada em outra business
    //verificar se essa pessoa já está cadastrada no sistema e apenas vincular a essa business e atribuir a role
    //apenas confirmar o cadastro pelo email do profissional
    //Depois de confirmado o cadastro como profissional ele poderá registrar serviços e hora no sistema

    private readonly IEFBaseRepository<Business> _businessrepository;
    private readonly IAuthService _authService;

    public CreateProfessionalCommandHandler(
        IEFBaseRepository<Business> businessrepository,
        IAuthService authService)
    {
        _businessrepository = businessrepository;
        _authService = authService;
    }

    public async Task<Result> Handle(CreateProfessionalCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

        var businessSpec = new EntityByIdSpecification<Business>(request.BusinessId);

        var business = await _businessrepository.GetFirstOrDefaultAsync(businessSpec, true, cancellationToken);

        return Result.Success();
    }
}
