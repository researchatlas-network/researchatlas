using System;
using System.Collections.Generic;
using System.Text;

namespace ResearchAtlas.Domain.Abstractions.Rules.BuiltIn
{   
    /// <summary>
    /// Business rule that validates a string property to ensure it meets a specified minimum length.
    /// </summary>
    public sealed class MinLengthStringRule : IBusinessRule
    {
        private readonly string _value;
        private readonly int _minLength;
        private readonly string _propertyName;

        /// <summary>
        /// Gets the rule name.
        /// </summary>
        public string RuleName => $"{_propertyName}.MinLength";

        /// <summary>
        /// Gets the error message if the rule is broken.
        /// </summary>
        public string ErrorMessage => $"{_propertyName} must be at least {_minLength} characters long. Current length: {_value?.Length ?? 0}.";

        /// <summary>
        /// Initializes a new instance of the <see cref="MinLengthStringRule"/> class.
        /// </summary>
        /// <param name="value">The string value to validate.</param>
        /// <param name="minLength">The minimum required length.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <exception cref="ArgumentNullException">Thrown when propertyName is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when minLength is less than or equal to zero.</exception>
        public MinLengthStringRule(string value, int minLength, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName), "Property name cannot be null or empty.");

            if (minLength <= 0)
                throw new ArgumentException("Min length must be greater than zero.", nameof(minLength));

            _value = value;
            _minLength = minLength;
            _propertyName = propertyName;
        }

        /// <summary>
        /// Evaluates whether the rule is satisfied (string meets the minimum length).
        /// </summary>
        /// <returns>true if the string is shorter than the minimum length; otherwise, false.</returns>
        public bool IsBroken() => string.IsNullOrEmpty(_value) || _value.Length < _minLength;

        /// <summary>
        /// Asynchronously evaluates whether the rule is satisfied.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(IsBroken());
    }
}
