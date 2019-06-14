using Shopify.Examples.Helpers;
using Shopify.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace goorinAR
{
    public class CartQuantityChangedEvent : UnityEvent<int>
    {
    }

    public class CheckoutFailureEvent : UnityEvent<string>
    {
    }

    public class HatCartPanel : MonoBehaviour
    {
        private Cart _cart;

        public CartQuantityChangedEvent OnCartQuantityChanged = new CartQuantityChangedEvent();
        public CheckoutFailureEvent OnCheckoutFailure = new CheckoutFailureEvent();
        public UnityEvent OnCheckoutSuccess;
        public UnityEvent OnCheckoutCancelled;
        public UnityEvent OnReturnToProducts;

        public Button BackToProductsButton;
        public Button CheckoutButton;
        public Text CheckoutButtonText;

        public HatCartPanelItem HatCartPanelItem;
        public RectTransform Content;

        private readonly Dictionary<string, ProductVariant> _idVariantMapping = new Dictionary<string, ProductVariant>();
        private readonly Dictionary<string, Product> _idProductMapping = new Dictionary<string, Product>();
        private readonly Dictionary<string, HatCartPanelItem> _idCartPanelLineItemMapping = new Dictionary<string, HatCartPanelItem>();
        private readonly List<HatCartPanelItem> _lineItems = new List<HatCartPanelItem>();

        public void Start()
        {
            BackToProductsButton.onClick.AddListener(() => OnReturnToProducts.Invoke());
            CheckoutButton.onClick.AddListener(() => {
                _cart.CheckoutWithWebView(
                    () => {
                        OnCheckoutSuccess.Invoke();
                        EmptyCart();
                    },
                    () => {
                        OnCheckoutCancelled.Invoke();
                    },
                    (checkoutError) => {
                        OnCheckoutFailure.Invoke(checkoutError.Description);
                    }
                );
            });
        }

        public void AddToCart(Product product, ProductVariant variant)
        {
            if (_cart == null)
            {
                _cart = ShopifyHelper.CreateCart();
            }

            var existingLineItem = _cart.LineItems.Get(variant);
            if (existingLineItem == null)
            {
                _cart.LineItems.AddOrUpdate(variant);
                var instance = Instantiate(HatCartPanelItem);
                instance.transform.SetParent(Content, false);
                instance.SetCurrentProduct(product, variant, 1);
                instance.OnVariantLineItemQuantityAdjustment.AddListener(HandleVariantLineItemQuantityAdjustment);
                _idCartPanelLineItemMapping.Add(variant.id(), instance);
                _lineItems.Add(instance);
            }
            else
            {
                _cart.LineItems.AddOrUpdate(variant, existingLineItem.Quantity + 1);

                var cartPanelLineItem = _idCartPanelLineItemMapping[variant.id()];
                cartPanelLineItem.Quantity.text = existingLineItem.Quantity.ToString();
            }
            if (!_idVariantMapping.ContainsKey(variant.id()))
            {
                _idVariantMapping.Add(variant.id(), variant);
            }

            if (!_idProductMapping.ContainsKey(variant.id()))
            {
                _idProductMapping.Add(variant.id(), product);
            }

            DispatchCartQuantityChanged();
           // UpdateSeparatorVisibility();

        }

        private void HandleVariantLineItemQuantityAdjustment(ProductVariant variant, int quantityAdjustment)
        {
            var lineItem = _cart.LineItems.Get(variant);
            _cart.LineItems.AddOrUpdate(variant, lineItem.Quantity + quantityAdjustment);
            var cartPanelLineItem = _idCartPanelLineItemMapping[variant.id()];
            if (lineItem.Quantity < 1)
            {
                Destroy(cartPanelLineItem.gameObject);
                _cart.LineItems.Delete(variant);
                _lineItems.Remove(cartPanelLineItem);
                _idCartPanelLineItemMapping.Remove(variant.id());
            }
            else
            {
                cartPanelLineItem.Quantity.text = lineItem.Quantity.ToString();
            }
            DispatchCartQuantityChanged();
        }

        public void EmptyCart()
        {
            _cart.Reset();

            foreach (var lineItem in _lineItems)
            {
                Destroy(lineItem.gameObject);
            }

            _idCartPanelLineItemMapping.Clear();

            DispatchCartQuantityChanged();
        }

        private void DispatchCartQuantityChanged()
        {
            var totalLineItemQuantity = 0;
            foreach (var lineItem in _cart.LineItems.All())
            {
                totalLineItemQuantity += (int)lineItem.Quantity;
            }

            CheckoutButtonText.text = "Checkout " + _cart.Subtotal().ToString("c");

            OnCartQuantityChanged.Invoke(totalLineItemQuantity);
        }
    }
}
