// <copyright file="BlockVisitor.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Provider
{
    using System.Linq;
    using System.Linq.Expressions;

    internal class QueryVisitor : ExpressionVisitor
    {
        private Expression newExpression;
        private bool changeResource;
        private int index = -1;

        public Expression ReplaceResource(QueryExpression query, Expression newResource, int index)
        {
            this.newExpression = newResource;
            this.index = index;
            this.changeResource = true;

            var returnEx = this.Visit(query);
            this.index = -1;
            return returnEx;
        }

        public Expression AddResouce(QueryExpression query, Expression resouce)
        {
            this.newExpression = resouce;
            this.changeResource = true;
            return this.Visit(query);
        }

        public Expression AddParameter(QueryExpression query, Expression parameter)
        {
            this.newExpression = parameter;
            return this.Visit(query);
        }

        protected override Expression VisitExtension(Expression node)
        {
            var query = node as QueryExpression;
            if (query == null)
            {
                return base.VisitExtension(node);
            }

            if (this.changeResource)
            {
                var location = query.Resource.ToList();
                if (location.Count == 1 && location[0] is DefaultExpression)
                {
                    location.Clear();
                }

                if (this.index > -1)
                {
                    location.RemoveAt(this.index);
                    location.Insert(this.index, this.newExpression);
                }

                location.Add(this.newExpression);

                return new QueryExpression(location, query.Parameters);
            }
            else
            {
                // ToDo: Implement duplicate replacement
                var queryParams = query.Parameters.ToList();
                queryParams.Add(this.newExpression);
                return new QueryExpression(query.Resource, queryParams);
            }
        }
    }
}