﻿using BeautyGo.Domain.Core.Primitives;

namespace BeautyGo.Domain.Core.Exceptions;

public class DomainException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class.
    /// </summary>
    /// <param name="error">The error containing the information about what happened.</param>
    public DomainException(Error error)
        : base(error.Message)
        => Error = error;

    /// <summary>
    /// Gets the error.
    /// </summary>
    public Error Error { get; }
}
