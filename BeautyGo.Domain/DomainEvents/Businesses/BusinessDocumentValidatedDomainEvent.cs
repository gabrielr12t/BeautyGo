﻿using BeautyGo.Domain.Core.Events;
using BeautyGo.Domain.Entities.Businesses;

namespace BeautyGo.Domain.DomainEvents.Businesses;

public record BusinessDocumentValidatedDomainEvent(Business Entity) : IDomainEvent;
