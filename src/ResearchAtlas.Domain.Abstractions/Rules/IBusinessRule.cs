using System.Threading;
using System.Threading.Tasks;

namespace ResearchAtlas.Domain.Abstractions.Rules
{
    /// <summary>
    /// Represents a business rule that can be evaluated.
    /// </summary>
    public interface IBusinessRule
    {
        /// <summary>
        /// Gets the rule name.
        /// </summary>
        string RuleName { get; }

        /// <summary>
        /// Gets the error message if the rule is broken.
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// Evaluates whether the rule is satisfied.
        /// </summary>
        /// <returns>true if the rule is satisfied; otherwise, false.</returns>
        bool IsBroken();

        /// <summary>
        /// Asynchronously evaluates whether the rule is satisfied.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. Returns true if the rule is satisfied; otherwise, false.</returns>
        Task<bool> IsBrokenAsync(CancellationToken cancellationToken = default);
    }
}
