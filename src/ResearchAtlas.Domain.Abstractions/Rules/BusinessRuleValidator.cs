using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResearchAtlas.Domain.Abstractions.Rules
{
    /// <summary>
    /// Validates a collection of business rules.
    /// </summary>
    public sealed class BusinessRuleValidator
    {
        /// <summary>
        /// Validates the specified business rules.
        /// </summary>
        /// <param name="rules">The rules to validate.</param>
        /// <exception cref="BusinessRuleException">Thrown when one or more rules are broken.</exception>
        public static void Validate(params IBusinessRule[] rules)
        {
            var brokenRules = rules
                .Where(rule => rule.IsBroken())
                .ToList();

            if (brokenRules.Any())
            {
                throw new BusinessRuleException(brokenRules);
            }
        }

        /// <summary>
        /// Validates the specified business rules.
        /// </summary>
        /// <param name="rules">The rules to validate.</param>
        /// <exception cref="BusinessRuleException">Thrown when one or more rules are broken.</exception>
        public static void Validate(IEnumerable<IBusinessRule> rules)
        {
            Validate(rules.ToArray());
        }

        /// <summary>
        /// Asynchronously validates the specified business rules.
        /// </summary>
        /// <param name="rules">The rules to validate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="BusinessRuleException">Thrown when one or more rules are broken.</exception>
        public static async Task ValidateAsync(
            IEnumerable<IBusinessRule> rules,
            CancellationToken cancellationToken = default)
        {
            var rulesList = rules.ToList();
            var brokenRules = new List<IBusinessRule>();

            foreach (var rule in rulesList)
            {
                if (await rule.IsBrokenAsync(cancellationToken))
                {
                    brokenRules.Add(rule);
                }
            }

            if (brokenRules.Any())
            {
                throw new BusinessRuleException(brokenRules);
            }
        }
    }
}
