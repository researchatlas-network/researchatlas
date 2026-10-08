using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ResearchAtlas.Domain.Abstractions.Specifications
{
    public abstract class Specification<T> : ISpecification<T>
    {
        private Func<T, bool>? _compiledExpression;

        public abstract Expression<Func<T, bool>> ToExpression();

        public bool IsSatisfiedBy(T candidate)
        {
            ArgumentNullException.ThrowIfNull(candidate);

            _compiledExpression ??= ToExpression().Compile();

            return _compiledExpression(candidate);
        }

        public Specification<T> And(ISpecification<T> other)
        {
            ArgumentNullException.ThrowIfNull(other);

            return new AndSpecification<T>(this, other);
        }

        public Specification<T> Or(ISpecification<T> other)
        {
            ArgumentNullException.ThrowIfNull(other);

            return new OrSpecification<T>(this, other);
        }

        public Specification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }
}
