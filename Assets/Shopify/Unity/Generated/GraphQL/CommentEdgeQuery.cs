namespace Shopify.Unity.GraphQL {
    using System;
    using System.Text;
    using System.Collections.Generic;
    using Shopify.Unity.SDK;

    public delegate void CommentEdgeDelegate(CommentEdgeQuery query);

    public class CommentEdgeQuery {
        private StringBuilder Query;

        /// <summary>
        /// <see cref="CommentEdgeQuery" /> is used to build queries. Typically
        /// <see cref="CommentEdgeQuery" /> will not be used directly but instead will be used when building queries from either
        /// <see cref="QueryRootQuery" /> or <see cref="MutationQuery" />.
        /// </summary>
        public CommentEdgeQuery(StringBuilder query) {
            Query = query;
        }

        /// <summary>
        /// A cursor for use in pagination.
        /// </summary>
        public CommentEdgeQuery cursor() {
            Query.Append("cursor ");

            return this;
        }

        /// <summary>
        /// The item at the end of CommentEdge.
        /// </summary>
        public CommentEdgeQuery node(CommentDelegate buildQuery) {
            Query.Append("node ");

            Query.Append("{");
            buildQuery(new CommentQuery(Query));
            Query.Append("}");

            return this;
        }
    }
    }
