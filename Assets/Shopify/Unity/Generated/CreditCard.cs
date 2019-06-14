namespace Shopify.Unity {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Shopify.Unity.SDK;

    /// <summary>
    /// Credit card information used for a payment.
    /// </summary>
    public class CreditCard : AbstractResponse, ICloneable {
        /// <summary>
        /// <see ref="CreditCard" /> Accepts deserialized json data.
        /// <see ref="CreditCard" /> Will further parse passed in data.
        /// </summary>
        /// <param name="dataJSON">Deserialized JSON data for CreditCard</param>
        public CreditCard(Dictionary<string, object> dataJSON) {
            DataJSON = dataJSON;
            Data = new Dictionary<string,object>();

            foreach (string key in dataJSON.Keys) {
                string fieldName = key;
                Regex regexAlias = new Regex("^(.+)___.+$");
                Match match = regexAlias.Match(key);

                if (match.Success) {
                    fieldName = match.Groups[1].Value;
                }

                switch(fieldName) {
                    case "brand":

                    if (dataJSON[key] == null) {
                        Data.Add(key, null);
                    } else {
                        Data.Add(
                            key,

                            (string) dataJSON[key]
                        );
                    }

                    break;

                    case "expiryMonth":

                    if (dataJSON[key] == null) {
                        Data.Add(key, null);
                    } else {
                        Data.Add(
                            key,

                            (long) dataJSON[key]
                        );
                    }

                    break;

                    case "expiryYear":

                    if (dataJSON[key] == null) {
                        Data.Add(key, null);
                    } else {
                        Data.Add(
                            key,

                            (long) dataJSON[key]
                        );
                    }

                    break;

                    case "firstDigits":

                    if (dataJSON[key] == null) {
                        Data.Add(key, null);
                    } else {
                        Data.Add(
                            key,

                            (string) dataJSON[key]
                        );
                    }

                    break;

                    case "firstName":

                    if (dataJSON[key] == null) {
                        Data.Add(key, null);
                    } else {
                        Data.Add(
                            key,

                            (string) dataJSON[key]
                        );
                    }

                    break;

                    case "lastDigits":

                    if (dataJSON[key] == null) {
                        Data.Add(key, null);
                    } else {
                        Data.Add(
                            key,

                            (string) dataJSON[key]
                        );
                    }

                    break;

                    case "lastName":

                    if (dataJSON[key] == null) {
                        Data.Add(key, null);
                    } else {
                        Data.Add(
                            key,

                            (string) dataJSON[key]
                        );
                    }

                    break;

                    case "maskedNumber":

                    if (dataJSON[key] == null) {
                        Data.Add(key, null);
                    } else {
                        Data.Add(
                            key,

                            (string) dataJSON[key]
                        );
                    }

                    break;
                }
            }
        }

        public string brand() {
            return Get<string>("brand");
        }

        public long? expiryMonth() {
            return Get<long?>("expiryMonth");
        }

        public long? expiryYear() {
            return Get<long?>("expiryYear");
        }

        public string firstDigits() {
            return Get<string>("firstDigits");
        }

        public string firstName() {
            return Get<string>("firstName");
        }

        public string lastDigits() {
            return Get<string>("lastDigits");
        }

        public string lastName() {
            return Get<string>("lastName");
        }

        /// <summary>
        /// Masked credit card number with only the last 4 digits displayed
        /// </summary>
        public string maskedNumber() {
            return Get<string>("maskedNumber");
        }

        public object Clone() {
            return new CreditCard(DataJSON);
        }

        private static List<Node> DataToNodeList(object data) {
            var objects = (List<object>)data;
            var nodes = new List<Node>();

            foreach (var obj in objects) {
                if (obj == null) continue;
                nodes.Add(UnknownNode.Create((Dictionary<string,object>) obj));
            }

            return nodes;
        }
    }
    }
