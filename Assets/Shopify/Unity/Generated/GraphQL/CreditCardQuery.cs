namespace Shopify.Unity.GraphQL {
    using System;
    using System.Text;
    using System.Collections.Generic;
    using Shopify.Unity.SDK;

    public delegate void CreditCardDelegate(CreditCardQuery query);

    /// <summary>
    /// Credit card information used for a payment.
    /// </summary>
    public class CreditCardQuery {
        private StringBuilder Query;

        /// <summary>
        /// <see cref="CreditCardQuery" /> is used to build queries. Typically
        /// <see cref="CreditCardQuery" /> will not be used directly but instead will be used when building queries from either
        /// <see cref="QueryRootQuery" /> or <see cref="MutationQuery" />.
        /// </summary>
        public CreditCardQuery(StringBuilder query) {
            Query = query;
        }

        public CreditCardQuery brand() {
            Query.Append("brand ");

            return this;
        }

        public CreditCardQuery expiryMonth() {
            Query.Append("expiryMonth ");

            return this;
        }

        public CreditCardQuery expiryYear() {
            Query.Append("expiryYear ");

            return this;
        }

        public CreditCardQuery firstDigits() {
            Query.Append("firstDigits ");

            return this;
        }

        public CreditCardQuery firstName() {
            Query.Append("firstName ");

            return this;
        }

        public CreditCardQuery lastDigits() {
            Query.Append("lastDigits ");

            return this;
        }

        public CreditCardQuery lastName() {
            Query.Append("lastName ");

            return this;
        }

        /// <summary>
        /// Masked credit card number with only the last 4 digits displayed
        /// </summary>
        public CreditCardQuery maskedNumber() {
            Query.Append("maskedNumber ");

            return this;
        }
    }
    }
