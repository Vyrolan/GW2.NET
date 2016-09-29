// <copyright file="QueryBuilder.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class QueryBuilder
    {
        private readonly BlockVisitor blockVisitor;
        private BlockExpression queryExpression;

        public QueryBuilder()
        {
            this.blockVisitor = new BlockVisitor();

            var locationEx = Expression.Block(Expression.Empty());
            var paramEx = Expression.Block(Expression.Empty());

            this.queryExpression = Expression.Block(locationEx, paramEx);
        }

        public QueryBuilder AtLocation<TProperty>(Expression<Func<TProperty>> property)
        {
            // Get the value of the property as string
            string value = property.Compile()().ToString();

            this.AtLocation(value);

            return this;
        }

        public QueryBuilder AtLocation(string location)
        {
            var locCons = Expression.Constant(location, typeof(string));
            var updLocEx = this.blockVisitor.Append(this.queryExpression.Expressions[0], locCons);
            this.queryExpression = (BlockExpression)this.blockVisitor.Replace(this.queryExpression, 0, updLocEx);

            return this;
        }

        public QueryBuilder WithParameter<TProperty>(Expression<Func<TProperty>> lambda)
        {
            // Get the properties name, used as key
            var expression = lambda.Body as MemberExpression;
            if (expression == null)
            {
                throw new ArgumentException("Expression is not a property.");
            }

            string propName = expression.Member.Name;

            // Get the value of the property
            TProperty value = lambda.Compile()();

            this.WithParameter(propName, value);

            return this;
        }

        public QueryBuilder WithParameter<TValue>(string key, TValue value)
        {
            var paramEx = Expression.Constant(new KeyValuePair<string, TValue>(key, value), typeof(KeyValuePair<string, TValue>));
            var updParamEx = this.blockVisitor.Append(this.queryExpression.Expressions[1], paramEx);
            this.queryExpression = (BlockExpression)this.blockVisitor.Replace(this.queryExpression, 1, updParamEx);

            return this;
        }

        public Expression Build()
        {
            return this.queryExpression;
        }
    }
}