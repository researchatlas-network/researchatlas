using System;
using System.Threading;
using System.Threading.Tasks;

namespace ResearchAtlas.Domain.Abstractions.Rules.BuiltIn
{
    /// <summary>
    /// Business rule that validates a string does not exceed a maximum length.
    /// </summary>
    public sealed class MaxLengthStringRule : IBusinessRule
    {
        private readonly string _value;
        private readonly int _maxLength;
        private readonly string _propertyName;

        /// <summary>
        /// Gets the rule name.
        /// </summary>
        public string RuleName => $"{_propertyName}.MaxLength";

        /// <summary>
        /// Gets the error message if the rule is broken.
        /// </summary>
        public string ErrorMessage => $"{_propertyName} cannot exceed {_maxLength} characters. Current length: {_value?.Length ?? 0}.";

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxLengthStringRule"/> class.
        /// </summary>
        /// <param name="value">The string value to validate.</param>
        /// <param name="maxLength">The maximum allowed length.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <exception cref="ArgumentNullException">Thrown when propertyName is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown when maxLength is less than or equal to zero.</exception>
        public MaxLengthStringRule(string value, int maxLength, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName), "Property name cannot be null or empty.");

            if (maxLength <= 0)
                throw new ArgumentException("Max length must be greater than zero.", nameof(maxLength));

            _value = value;
            _maxLength = maxLength;
            _propertyName = propertyName;
        }

        /// <summary>
        /// Evaluates whether the rule is satisfied (string does not exceed max length).
        /// </summary>
        /// <returns>true if the string exceeds the maximum length; otherwise, false.</returns>
        public bool IsBroken() => !string.IsNullOrEmpty(_value) && _value.Length > _maxLength;

        /// <summary>
        /// Asynchronously evaluates whether the rule is satisfied.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(IsBroken());
    }
}
