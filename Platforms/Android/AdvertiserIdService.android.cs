using FacebookAppEvents.src;
using Google.Ads.Identifier;
using System;
using System.Threading.Tasks;

namespace FacebookAppEvents.Platforms.Android
{
    /// <summary>
    /// Android implementation of IAdvertiserIdService using Google Advertising ID (GAID).
    /// All operations are performed on background threads as required by the Android API.
    /// </summary>
    public class AdvertiserIdService : IAdvertiserIdService
    {
        /// <summary>
        /// Gets the Google Advertising ID (GAID) for Android devices.
        /// This method ensures execution on a background thread to comply with Android requirements.
        /// </summary>
        /// <returns>The GAID if available and tracking is enabled, null otherwise.</returns>
        public async Task<string?> GetAdvertiserIdAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var info = AdvertisingIdClient.GetAdvertisingIdInfo(global::Android.App.Application.Context);

                    if (info != null &&
                        !info.IsLimitAdTrackingEnabled &&
                        !string.IsNullOrEmpty(info.Id) &&
                        info.Id != "00000000-0000-0000-0000-000000000000")
                    {
                        return info.Id;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Android Advertising ID error: {ex.Message}");
                }

                return null;
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Checks if the user has enabled advertising tracking on Android.
        /// </summary>
        /// <returns>True if tracking is enabled, false if limited or unavailable.</returns>
        public async Task<bool> IsTrackingEnabledAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var info = AdvertisingIdClient.GetAdvertisingIdInfo(global::Android.App.Application.Context);
                    return info != null && !info.IsLimitAdTrackingEnabled;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Android tracking status error: {ex.Message}");
                    return false;
                }
            }).ConfigureAwait(false);
        }
    }
}