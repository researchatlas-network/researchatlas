using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ResearchAtlas.Domain.Abstractions.Specifications
{
    internal sealed class AndSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public AndSpecification(
            ISpecification<T> left,
            ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            return SpecificationExpression.Combine(
                _left.ToExpression(),
                _right.ToExpression(),
                Expression.AndAlso);
        }
    }
}
