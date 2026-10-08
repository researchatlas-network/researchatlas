using System;
using System.Threading;
using System.Threading.Tasks;

namespace ResearchAtlas.Domain.Abstractions.Rules.BuiltIn
{
    /// <summary>
    /// Business rule that validates a string is not null or empty.
    /// </summary>
    public sealed class NotEmptyStringRule : IBusinessRule
    {
        private readonly string _value;
        private readonly string _propertyName;

        /// <summary>
        /// Gets the rule name.
        /// </summary>
        public string RuleName => $"{_propertyName}.NotEmpty";

        /// <summary>
        /// Gets the error message if the rule is broken.
        /// </summary>
        public string ErrorMessage => $"{_propertyName} cannot be null or empty.";

        /// <summary>
        /// Initializes a new instance of the <see cref="NotEmptyStringRule"/> class.
        /// </summary>
        /// <param name="value">The string value to validate.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <exception cref="ArgumentNullException">Thrown when propertyName is null or empty.</exception>
        public NotEmptyStringRule(string value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName), "Property name cannot be null or empty.");

            _value = value;
            _propertyName = propertyName;
        }

        /// <summary>
        /// Evaluates whether the rule is satisfied (string is not null or empty).
        /// </summary>
        /// <returns>true if the string is null or empty; otherwise, false.</returns>
        public bool IsBroken() => string.IsNullOrEmpty(_value);

        /// <summary>
        /// Asynchronously evaluates whether the rule is satisfied.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(IsBroken());
    }
}
