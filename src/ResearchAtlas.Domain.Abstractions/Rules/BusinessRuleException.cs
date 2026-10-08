using System;
using System.Collections.Generic;
using System.Linq;
using ResearchAtlas.Domain.Abstractions.Exceptions;

namespace ResearchAtlas.Domain.Abstractions.Rules
{
    /// <summary>
    /// Exception thrown when a business rule is violated.
    /// </summary>
    public sealed class BusinessRuleException : DomainException
    {
        /// <summary>
        /// Gets the broken rules.
        /// </summary>
        public IReadOnlyList<IBusinessRule> BrokenRules { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessRuleException"/> class.
        /// </summary>
        /// <param name="brokenRule">The broken rule.</param>
        public BusinessRuleException(IBusinessRule brokenRule)
            : this(new[] { brokenRule })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessRuleException"/> class.
        /// </summary>
        /// <param name="brokenRules">The broken rules.</param>
        public BusinessRuleException(IEnumerable<IBusinessRule> brokenRules)
            : base(
                "BUSINESS_RULE_BROKEN",
                BuildMessage(brokenRules.ToList()))
        {
            BrokenRules = brokenRules.ToList().AsReadOnly();
        }

        private static string BuildMessage(List<IBusinessRule> brokenRules)
        {
            var errorMessages = brokenRules
                .Select(rule => $"{rule.RuleName}: {rule.ErrorMessage}")
                .ToList();

            return $"Business rule(s) violated: {string.Join("; ", errorMessages)}";
        }
    }
}
