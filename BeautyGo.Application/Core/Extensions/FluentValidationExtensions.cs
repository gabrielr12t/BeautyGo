﻿using FluentValidation;
using BeautyGo.Domain.Core.Primitives;

namespace BeautyGo.Application.Core.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule, Error error)
        {
            if (error is null)
                throw new ArgumentNullException(nameof(error), "The error is required");

            return rule.WithErrorCode(error.Code).WithMessage(error.Message);
        }
    }
}
