namespace Shopify.Unity.GraphQL {
    using System;
    using System.Text;
    using System.Collections.Generic;
    using Shopify.Unity.SDK;

    public delegate void SelectedOptionDelegate(SelectedOptionQuery query);

    /// <summary>
    /// Custom properties that a shop owner can use to define product variants.
    /// Multiple options can exist. Options are represented as: option1, option2, option3, etc.
    /// </summary>
    public class SelectedOptionQuery {
        private StringBuilder Query;

        /// <summary>
        /// <see cref="SelectedOptionQuery" /> is used to build queries. Typically
        /// <see cref="SelectedOptionQuery" /> will not be used directly but instead will be used when building queries from either
        /// <see cref="QueryRootQuery" /> or <see cref="MutationQuery" />.
        /// </summary>
        public SelectedOptionQuery(StringBuilder query) {
            Query = query;
        }

        /// <summary>
        /// The product option’s name.
        /// </summary>
        public SelectedOptionQuery name() {
            Query.Append("name ");

            return this;
        }

        /// <summary>
        /// The product option’s value.
        /// </summary>
        public SelectedOptionQuery value() {
            Query.Append("value ");

            return this;
        }
    }
    }
