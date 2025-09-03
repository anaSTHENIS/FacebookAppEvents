using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FacebookAppEvents.src
{
    /// <summary>
    /// Enhanced helper class to send Facebook App Events with automatic advertiser ID handling.
    /// </summary>
    public static class FacebookAppEventSender
    {
        private readonly HttpClient _httpClient;
        private readonly string _appId;
        private readonly string _clientToken;
        private readonly IAdvertiserIdService? _advertiserIdService;

        /// <summary>
        /// Initializes a new instance with manual advertiser ID handling.
        /// </summary>
        /// <param name="httpClient">An HttpClient instance for making API calls.</param>
        /// <param name="appId">Facebook App ID.</param>
        /// <param name="clientToken">Facebook Client Token.</param>
        public static FacebookAppEventSender(HttpClient httpClient, string appId, string clientToken)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _appId = appId ?? throw new ArgumentNullException(nameof(appId));
            _clientToken = clientToken ?? throw new ArgumentNullException(nameof(clientToken));
        }

        /// <summary>
        /// Initializes a new instance with automatic advertiser ID handling.
        /// </summary>
        /// <param name="httpClient">An HttpClient instance for making API calls.</param>
        /// <param name="appId">Facebook App ID.</param>
        /// <param name="clientToken">Facebook Client Token.</param>
        /// <param name="advertiserIdService">Platform-specific advertiser ID service.</param>
        public static FacebookAppEventSender(HttpClient httpClient, string appId, string clientToken, IAdvertiserIdService advertiserIdService)
            : this(httpClient, appId, clientToken)
        {
            _advertiserIdService = advertiserIdService ?? throw new ArgumentNullException(nameof(advertiserIdService));
        }

        /// <summary>
        /// Sends events with automatic advertiser ID retrieval (requires IAdvertiserIdService).
        /// </summary>
        /// <param name="events">The array of FacebookAppEvent to send.</param>
        /// <returns>Task returning true if successful; otherwise false.</returns>
        public static async Task<bool> SendEventsAsync(params FacebookAppEvent[] events)
        {
            if (_advertiserIdService == null)
                throw new InvalidOperationException("IAdvertiserIdService must be provided to use automatic advertiser ID retrieval.");

            string? advertiserId = await _advertiserIdService.GetAdvertiserIdAsync();
            bool trackingEnabled = await _advertiserIdService.IsTrackingEnabledAsync();

            return await SendEventsAsync(advertiserId ?? string.Empty, trackingEnabled, events);
        }

        /// <summary>
        /// Sends events with manually provided advertiser ID and tracking status.
        /// </summary>
        /// <param name="advertiserId">The advertiser ID (IDFA/GAID). Pass empty string if unavailable.</param>
        /// <param name="advertiserTrackingEnabled">Whether advertiser tracking is enabled.</param>
        /// <param name="events">The array of FacebookAppEvent to send.</param>
        /// <returns>Task returning true if successful; otherwise false.</returns>
        public static async Task<bool> SendEventsAsync(string advertiserId, bool advertiserTrackingEnabled, params FacebookAppEvent[] events)
        {
            if (events == null || events.Length == 0)
                throw new ArgumentNullException(nameof(events), "At least one FacebookAppEvent must be provided.");

            var url = $"https://graph.facebook.com/v23.0/{_appId}/activities";

            var parameters = new Dictionary<string, string>
            {
                { "event", "CUSTOM_APP_EVENTS" },
                { "app_id", _appId },
                { "client_token", _clientToken },
                { "advertiser_id", advertiserId ?? string.Empty },
                { "advertiser_tracking_enabled", advertiserTrackingEnabled ? "1" : "0" },
                { "application_tracking_enabled", advertiserTrackingEnabled ? "1" : "0" },
                { "custom_events", JsonSerializer.Serialize(events) }
            };

            using var content = new FormUrlEncodedContent(parameters);
            using var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Facebook API Error: {response.StatusCode} - {responseContent}");
                return false;
            }

            return true;
        }
    }
}
