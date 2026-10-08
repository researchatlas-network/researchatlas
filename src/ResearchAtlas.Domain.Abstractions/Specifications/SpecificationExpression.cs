using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ResearchAtlas.Domain.Abstractions.Specifications
{
    internal static class SpecificationExpression
    {
        public static Expression<Func<T, bool>> Combine<T>(
            Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right,
            Func<Expression, Expression, BinaryExpression> merge)
        {
            var parameter = Expression.Parameter(typeof(T), "entity");

            var leftBody = new ParameterReplaceVisitor(
                    left.Parameters[0],
                    parameter)
                .Visit(left.Body)!;

            var rightBody = new ParameterReplaceVisitor(
                    right.Parameters[0],
                    parameter)
                .Visit(right.Body)!;

            return Expression.Lambda<Func<T, bool>>(
                merge(leftBody, rightBody),
                parameter);
        }

        private sealed class ParameterReplaceVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _source;
            private readonly ParameterExpression _target;

            public ParameterReplaceVisitor(
                ParameterExpression source,
                ParameterExpression target)
            {
                _source = source;
                _target = target;
            }

            protected override Expression VisitParameter(
                ParameterExpression node)
            {
                return node == _source
                    ? _target
                    : base.VisitParameter(node);
            }
        }
    }
}
