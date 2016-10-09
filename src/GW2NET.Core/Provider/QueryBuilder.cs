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
        private readonly QueryVisitor blockVisitor;
        private QueryExpression query;

        public QueryBuilder()
        {
            this.blockVisitor = new QueryVisitor();

            this.query = new QueryExpression(Expression.Empty());
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
            this.query = (QueryExpression)this.blockVisitor.AddResouce(this.query, locCons);

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
            this.query = (QueryExpression)this.blockVisitor.AddResouce(this.query, paramEx);

            return this;
        }

        public Expression Build()
        {
            return this.query;
        }
    }
}