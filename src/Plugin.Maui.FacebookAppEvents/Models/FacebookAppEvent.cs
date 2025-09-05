using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Plugin.Maui.FacebookAppEvents.Models
{
    /// <summary>
    /// Represents a Facebook App Event that can be sent via the Facebook Graph API.
    /// Contains all necessary properties for tracking user actions and conversions.
    /// </summary>
    public class FacebookAppEvent
    {
        /// <summary>
        /// The name of the event (e.g., "fb_mobile_purchase", "fb_mobile_add_to_cart").
        /// This field is required by Facebook's API.
        /// </summary>
        [JsonPropertyName("_eventName")]
        public string EventName { get; set; }

        /// <summary>
        /// A unique identifier for this specific event instance.
        /// If not explicitly set, a new GUID will be generated automatically.
        /// </summary>
        [JsonPropertyName("event_id")]
        public string EventId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The list of content items involved in this event (e.g., products, screens).
        /// Optional, depending on the event type. Used for purchase, add to cart, etc.
        /// </summary>
        [JsonPropertyName("fb_content")]
        public List<FacebookContentItem> FbContent { get; set; }

        /// <summary>
        /// The type of content being tracked (e.g., "product", "screen", "user_action").
        /// Optional field that helps categorize the event content.
        /// </summary>
        [JsonPropertyName("fb_content_type")]
        public string FbContentType { get; set; }

        /// <summary>
        /// The monetary value associated with this event (e.g., purchase amount).
        /// Optional. Used primarily for conversion value optimization.
        /// </summary>
        [JsonPropertyName("_valueToSum")]
        public decimal? ValueToSum { get; set; }

        /// <summary>
        /// The currency code for the ValueToSum (e.g., "USD", "EUR", "GBP").
        /// Optional. Should be a 3-letter ISO 4217 currency code.
        /// </summary>
        [JsonPropertyName("fb_currency")]
        public string FbCurrency { get; set; }
    }

    /// <summary>
    /// Represents an individual content item involved in a Facebook App Event.
    /// Typically used to represent products, articles, or other trackable items.
    /// </summary>
    public class FacebookContentItem
    {
        /// <summary>
        /// The unique identifier for this content item (e.g., product ID, article ID).
        /// This should be a meaningful identifier from your system.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// The quantity of this content item involved in the event.
        /// For example, if 3 units of a product were added to cart, this would be 3.
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
    }
}
