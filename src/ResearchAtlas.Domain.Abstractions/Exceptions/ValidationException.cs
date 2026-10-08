using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAtlas.Domain.Abstractions.Exceptions
{
    /// <summary>
    /// Exception thrown when domain validation fails.
    /// </summary>
    public sealed class ValidationException : DomainException
    {
        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        public IReadOnlyDictionary<string, string[]> Errors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="errors">The validation errors.</param>
        public ValidationException(Dictionary<string, string[]> errors)
            : base(
                "VALIDATION_ERROR",
                BuildMessage(errors))
        {
            Errors = errors.AsReadOnly();
        }

        private static string BuildMessage(Dictionary<string, string[]> errors)
        {
            var errorMessages = errors
                .SelectMany(kvp => kvp.Value.Select(msg => $"{kvp.Key}: {msg}"))
                .ToList();

            return $"Validation failed with {errorMessages.Count} error(s): {string.Join("; ", errorMessages)}";
        }
    }
}
