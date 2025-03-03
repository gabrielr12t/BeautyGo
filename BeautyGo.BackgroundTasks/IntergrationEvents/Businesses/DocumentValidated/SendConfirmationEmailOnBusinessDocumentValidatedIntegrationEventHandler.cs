using BeautyGo.Application.Businesses.Commands.BusinessDocumentValidated;
using BeautyGo.Application.Core.Abstractions.Data;
using BeautyGo.Application.Core.Abstractions.Logging;
using BeautyGo.BackgroundTasks.Abstractions.Messaging;
using BeautyGo.Domain.Entities.Businesses;
using BeautyGo.Domain.Patterns.Specifications;
using BeautyGo.Domain.Repositories;

namespace BeautyGo.BackgroundTasks.IntergrationEvents.Businesses.DocumentValidated;

internal class SendConfirmationEmailOnBusinessDocumentValidatedIntegrationEventHandler
    : IIntegrationEventHandler<BusinessDocumentValidatedIntegrationEvent>
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<Business> _businessRepository;

    public SendConfirmationEmailOnBusinessDocumentValidatedIntegrationEventHandler(
        ILogger logger,
        IUnitOfWork unitOfWork,
        IBaseRepository<Business> businessRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _businessRepository = businessRepository;
    }

    public async Task Handle(BusinessDocumentValidatedIntegrationEvent notification, CancellationToken cancellationToken)
    {

        //var userByIdSpecification = new EntityByIdSpecification<User>(notification.UserId);
        //var user = await _userRepository.GetFirstOrDefaultAsync(userByIdSpecification);

        //var message = new WelcomeEmail(user.Email, user.FullName());

        //await _emailNotificationService.SendAsync(message);

        var business = await _businessRepository.GetFirstOrDefaultAsync(new EntityByIdSpecification<Business>(notification.BusinessId));

        await _logger.InformationAsync($"Enviando email de confirmação para o cnpj criado: {business.Cnpj}");

        await _unitOfWork.SaveChangesAsync();
    }
}
