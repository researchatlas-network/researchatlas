using System;
using System.Threading;
using System.Threading.Tasks;

namespace ResearchAtlas.Domain.Abstractions.Rules.BuiltIn
{
    /// <summary>
    /// Business rule that validates a nullable value type is not null.
    /// </summary>
    /// <typeparam name="T">The nullable value type to validate.</typeparam>
    public sealed class NotNullValueRule<T> : IBusinessRule
        where T : struct
    {
        private readonly T? _value;
        private readonly string _propertyName;

        /// <summary>
        /// Gets the rule name.
        /// </summary>
        public string RuleName => $"{_propertyName}.NotNull";

        /// <summary>
        /// Gets the error message if the rule is broken.
        /// </summary>
        public string ErrorMessage => $"{_propertyName} cannot be null.";

        /// <summary>
        /// Initializes a new instance of the <see cref="NotNullValueRule{T}"/> class.
        /// </summary>
        /// <param name="value">The nullable value to validate.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <exception cref="ArgumentNullException">Thrown when propertyName is null or empty.</exception>
        public NotNullValueRule(T? value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName), "Property name cannot be null or empty.");

            _value = value;
            _propertyName = propertyName;
        }

        /// <summary>
        /// Evaluates whether the rule is satisfied (value is not null).
        /// </summary>
        /// <returns>true if the value is null; otherwise, false.</returns>
        public bool IsBroken() => !_value.HasValue;

        /// <summary>
        /// Asynchronously evaluates whether the rule is satisfied.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(IsBroken());
    }
}
