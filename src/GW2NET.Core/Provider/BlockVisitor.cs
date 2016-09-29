// <copyright file="BlockVisitor.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Provider
{
    using System.Linq;
    using System.Linq.Expressions;

    internal class BlockVisitor : ExpressionVisitor
    {
        private Expression expression;

        private int replaceIndex = -1;

        /// <summary>Replaces a child in the block expression at the given index.</summary>
        public Expression Replace(Expression block, int index, Expression replacingExpression)
        {
            this.expression = replacingExpression;
            this.replaceIndex = index;

            var returnEx = this.Visit(block);

            this.replaceIndex = -1;
            return returnEx;
        }

        /// <summary>Appends a child to the given block expression.</summary>
        public Expression Append(Expression block, Expression appendingExpression)
        {
            this.expression = appendingExpression;

            return this.Visit(block);
        }

        /// <inheritdoc />
        protected override Expression VisitBlock(BlockExpression node)
        {
            if (this.expression == null)
            {
                return base.VisitBlock(node);
            }

            if (this.replaceIndex == -1)
            {
                var nodeChildren = node.Expressions.ToList();

                if (nodeChildren.Count == 1 && nodeChildren.First().Type == typeof(void))
                {
                    nodeChildren.Clear();
                }

                nodeChildren.Add(this.expression);

                return Expression.Block(nodeChildren);
            }
            else
            {
                var nodeChildren = node.Expressions.ToList();
                nodeChildren[this.replaceIndex] = this.expression;

                return Expression.Block(nodeChildren);
            }
        }
    }
}