using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ResearchAtlas.Domain.Abstractions.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();

        bool IsSatisfiedBy(T candidate);
    }
}
