// <copyright file="QueryExpression.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq.Expressions;

    public sealed class QueryExpression : Expression
    {
        public QueryExpression(Expression resource)
        {
            this.Resource = new ReadOnlyCollection<Expression>(new List<Expression>(1) { resource });
        }

        public QueryExpression(IList<Expression> resource)
        {
            this.Resource = new ReadOnlyCollection<Expression>(resource);
        }

        public override bool CanReduce => false;

        public override ExpressionType NodeType => ExpressionType.Extension;

        public ReadOnlyCollection<Expression> Resource { get; }

        public Expression Parameters { get; }

        public override Type Type => this.GetType();
    }
}
