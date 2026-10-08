using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResearchAtlas.Domain.Abstractions.Rules.BuiltIn
{
    /// <summary>
    /// Business rule that validates a collection is not empty.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public sealed class ListNotEmptyRule<T> : IBusinessRule
    {
        private readonly IEnumerable<T> _collection;
        private readonly string _propertyName;

        /// <summary>
        /// Gets the rule name.
        /// </summary>
        public string RuleName => $"{_propertyName}.NotEmpty";

        /// <summary>
        /// Gets the error message if the rule is broken.
        /// </summary>
        public string ErrorMessage => $"{_propertyName} cannot be empty.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ListNotEmptyRule{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection to validate.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <exception cref="ArgumentNullException">Thrown when propertyName is null or empty.</exception>
        public ListNotEmptyRule(IEnumerable<T> collection, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName), "Property name cannot be null or empty.");

            _collection = collection;
            _propertyName = propertyName;
        }

        /// <summary>
        /// Evaluates whether the rule is satisfied (collection is not empty).
        /// </summary>
        /// <returns>true if the collection is null or empty; otherwise, false.</returns>
        public bool IsBroken() => _collection is null || !_collection.Any();

        /// <summary>
        /// Asynchronously evaluates whether the rule is satisfied.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(IsBroken());
    }
}
