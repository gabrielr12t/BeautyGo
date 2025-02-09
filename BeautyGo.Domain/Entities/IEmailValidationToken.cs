using BeautyGo.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyGo.Domain.Entities;

public interface IEmailValidationToken : IDomainEvent
{
    BeautyGoEmailTokenValidation CreateEmailValidationToken();
}
