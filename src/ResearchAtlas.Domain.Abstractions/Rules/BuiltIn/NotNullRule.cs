using System;
using System.Threading;
using System.Threading.Tasks;

namespace ResearchAtlas.Domain.Abstractions.Rules.BuiltIn
{
    /// <summary>
    /// Business rule that validates an object is not null.
    /// </summary>
    /// <typeparam name="T">The type of the object to validate.</typeparam>
    public sealed class NotNullRule<T> : IBusinessRule
        where T : class
    {
        private readonly T _value;
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
        /// Initializes a new instance of the <see cref="NotNullRule{T}"/> class.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <exception cref="ArgumentNullException">Thrown when propertyName is null or empty.</exception>
        public NotNullRule(T value, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName), "Property name cannot be null or empty.");

            _value = value;
            _propertyName = propertyName;
        }

        /// <summary>
        /// Evaluates whether the rule is satisfied (value is not null).
        /// </summary>
        /// <returns>true if the value is not null; otherwise, false.</returns>
        public bool IsBroken() => _value is null;

        /// <summary>
        /// Asynchronously evaluates whether the rule is satisfied.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(IsBroken());
    }
}
