using System;
using System.Collections.Generic;

namespace FacebookAppEvents
{
    /// <summary>
    /// Factory class for creating common Facebook App Events with sensible defaults.
    /// Provides pre-configured methods for standard e-commerce and app events.
    /// </summary>
    public static class FacebookAppEventFactory
    {
        /// <summary>
        /// Creates a purchase event for tracking completed transactions.
        /// </summary>
        /// <param name="contents">The list of content items purchased.</param>
        /// <param name="valueToSum">The total monetary value of the purchase.</param>
        /// <param name="currency">The currency code (e.g., "USD", "EUR").</param>
        /// <param name="eventId">Optional custom event ID. If null, auto-generated with "purchase-" prefix.</param>
        /// <returns>A FacebookAppEvent representing a completed purchase.</returns>
        /// <example>
        /// <code>
        /// var items = new List&lt;FacebookContentItem&gt;
        /// {
        ///     new() { Id = "product_123", Quantity = 2 }
        /// };
        /// var purchaseEvent = FacebookAppEventFactory.CreatePurchaseEvent(items, 59.98, "USD");
        /// </code>
        /// </example>
        public static FacebookAppEvent CreatePurchaseEvent(
            List<FacebookContentItem> contents,
            double valueToSum,
            string currency,
            string? eventId = null)
        {
            return new FacebookAppEvent
            {
                EventName = "fb_mobile_purchase",
                EventId = eventId ?? "purchase-" + Guid.NewGuid(),
                FbContent = contents,
                FbContentType = "product",
                ValueToSum = valueToSum,
                FbCurrency = currency
            };
        }

        /// <summary>
        /// Creates an add to cart event for tracking when users add items to their shopping cart.
        /// </summary>
        /// <param name="contents">The list of content items added to the cart.</param>
        /// <param name="eventId">Optional custom event ID. If null, auto-generated with "addtocart-" prefix.</param>
        /// <param name="contentType">Optional content type override, default is "product".</param>
        /// <param name="eventName">Optional event name override, default is "fb_mobile_add_to_cart".</param>
        /// <returns>A FacebookAppEvent representing items added to cart.</returns>
        /// <example>
        /// <code>
        /// var items = new List&lt;FacebookContentItem&gt;
        /// {
        ///     new() { Id = "product_456", Quantity = 1 }
        /// };
        /// var addToCartEvent = FacebookAppEventFactory.CreateAddToCartEvent(items);
        /// </code>
        /// </example>
        public static FacebookAppEvent CreateAddToCartEvent(
            List<FacebookContentItem> contents,
            string? eventId = null,
            string contentType = "product",
            string eventName = "fb_mobile_add_to_cart")
        {
            return new FacebookAppEvent
            {
                EventName = eventName,
                EventId = eventId ?? "addtocart-" + Guid.NewGuid(),
                FbContent = contents,
                FbContentType = contentType
            };
        }

        /// <summary>
        /// Creates a remove from cart event for tracking when users remove items from their cart.
        /// </summary>
        /// <param name="contents">The list of content items removed from the cart.</param>
        /// <param name="eventId">Optional custom event ID. If null, auto-generated with "removefromcart-" prefix.</param>
        /// <param name="contentType">Optional content type override, default is "product".</param>
        /// <param name="eventName">Optional event name override, default is "fb_mobile_remove_from_cart".</param>
        /// <returns>A FacebookAppEvent representing items removed from cart.</returns>
        public static FacebookAppEvent CreateRemoveFromCartEvent(
            List<FacebookContentItem> contents,
            string? eventId = null,
            string contentType = "product",
            string eventName = "fb_mobile_remove_from_cart")
        {
            return new FacebookAppEvent
            {
                EventName = eventName,
                EventId = eventId ?? "removefromcart-" + Guid.NewGuid(),
                FbContent = contents,
                FbContentType = contentType
            };
        }

        /// <summary>
        /// Creates a screen view event for tracking when users view specific screens or pages.
        /// </summary>
        /// <param name="screenName">The name or identifier of the screen viewed.</param>
        /// <param name="eventId">Optional custom event ID. If null, auto-generated with "screenview-" prefix.</param>
        /// <param name="contentType">Optional content type override, default is "screen".</param>
        /// <param name="eventName">Optional event name override, default is "fb_mobile_content_view".</param>
        /// <returns>A FacebookAppEvent representing a screen view.</returns>
        /// <example>
        /// <code>
        /// var screenEvent = FacebookAppEventFactory.CreateScreenViewEvent("HomeScreen");
        /// </code>
        /// </example>
        public static FacebookAppEvent CreateScreenViewEvent(
            string screenName,
            string? eventId = null,
            string contentType = "screen",
            string eventName = "fb_mobile_content_view")
        {
            return new FacebookAppEvent
            {
                EventName = eventName,
                EventId = eventId ?? "screenview-" + Guid.NewGuid(),
                FbContentType = contentType,
                FbContent = new List<FacebookContentItem>
                {
                    new() { Id = screenName, Quantity = 1 }
                }
            };
        }

        /// <summary>
        /// Creates a login/registration completion event for tracking user authentication.
        /// </summary>
        /// <param name="eventId">Optional custom event ID. If null, auto-generated with "login-" prefix.</param>
        /// <param name="eventName">Optional event name override, default is "fb_mobile_complete_registration".</param>
        /// <returns>A FacebookAppEvent representing user login or registration completion.</returns>
        /// <example>
        /// <code>
        /// var loginEvent = FacebookAppEventFactory.CreateLoginEvent();
        /// </code>
        /// </example>
        public static FacebookAppEvent CreateLoginEvent(
            string? eventId = null,
            string eventName = "fb_mobile_complete_registration")
        {
            return new FacebookAppEvent
            {
                EventName = eventName,
                EventId = eventId ?? "login-" + Guid.NewGuid()
            };
        }

        /// <summary>
        /// Creates a search event for tracking when users perform searches in the app.
        /// </summary>
        /// <param name="searchString">The search term or query used.</param>
        /// <param name="eventId">Optional custom event ID. If null, auto-generated with "search-" prefix.</param>
        /// <param name="contentType">Optional content type, default is "search".</param>
        /// <param name="eventName">Optional event name override, default is "fb_mobile_search".</param>
        /// <returns>A FacebookAppEvent representing a search action.</returns>
        public static FacebookAppEvent CreateSearchEvent(
            string searchString,
            string? eventId = null,
            string contentType = "search",
            string eventName = "fb_mobile_search")
        {
            return new FacebookAppEvent
            {
                EventName = eventName,
                EventId = eventId ?? "search-" + Guid.NewGuid(),
                FbContentType = contentType,
                FbContent = new List<FacebookContentItem>
                {
                    new() { Id = searchString, Quantity = 1 }
                }
            };
        }

        /// <summary>
        /// Creates a custom event with fully user-specified parameters.
        /// Use this when the pre-built factory methods don't meet your specific needs.
        /// </summary>
        /// <param name="eventName">The custom event name (required).</param>
        /// <param name="eventId">Optional custom event ID. If null, auto-generated.</param>
        /// <param name="contentType">Optional content type classification.</param>
        /// <param name="contents">Optional list of content items involved in the event.</param>
        /// <param name="valueToSum">Optional monetary value associated with the event.</param>
        /// <param name="currency">Optional currency code for the value.</param>
        /// <returns>A custom FacebookAppEvent with the specified parameters.</returns>
        /// <example>
        /// <code>
        /// var customEvent = FacebookAppEventFactory.CreateCustomEvent(
        ///     eventName: "user_shared_content",
        ///     eventId: "share_" + DateTime.UtcNow.Ticks,
        ///     contentType: "social_action",
        ///     contents: new List&lt;FacebookContentItem&gt;
        ///     {
        ///         new() { Id = "article_123", Quantity = 1 }
        ///     }
        /// );
        /// </code>
        /// </example>
        public static FacebookAppEvent CreateCustomEvent(
            string eventName,
            string? eventId = null,
            string? contentType = null,
            List<FacebookContentItem>? contents = null,
            double? valueToSum = null,
            string? currency = null)
        {
            if (string.IsNullOrWhiteSpace(eventName))
                throw new ArgumentException("Event name cannot be null or empty.", nameof(eventName));

            return new FacebookAppEvent
            {
                EventName = eventName,
                EventId = eventId ?? Guid.NewGuid().ToString(),
                FbContentType = contentType,
                FbContent = contents,
                ValueToSum = valueToSum,
                FbCurrency = currency
            };
        }
    }
}
