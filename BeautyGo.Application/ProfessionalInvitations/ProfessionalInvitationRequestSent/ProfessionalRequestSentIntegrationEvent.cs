﻿using BeautyGo.Application.Core.Abstractions.Messaging;

namespace BeautyGo.Application.ProfessionalInvitations.ProfessionalInvitationRequestSent;

public record ProfessionalRequestSentIntegrationEvent(Guid ProfessionalInvitationId) : IIntegrationEvent;
