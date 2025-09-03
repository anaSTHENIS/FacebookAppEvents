using AppTrackingTransparency;
using AdSupport;
using Foundation;
using FacebookAppEvents.src.Plugin.Maui.FacebookAppEvents.Services;

namespace FacebookAppEvents.Platforms.iOS
{
    /// <summary>
    /// iOS implementation of IAdvertiserIdService using IDFA with App Tracking Transparency.
    /// </summary>
    public class AdvertiserIdService : IAdvertiserIdService
    {
        /// <summary>
        /// Gets the iOS Identifier for Advertisers (IDFA).
        /// </summary>
        /// <returns>The IDFA if available and authorized, null otherwise.</returns>
        public async Task<string?> GetAdvertiserIdAsync()
        {
            try
            {
                if (NSProcessInfo.ProcessInfo.IsOperatingSystemAtLeastVersion(new NSOperatingSystemVersion(14, 0, 0)))
                {
                    var status = await ATTrackingManager.RequestTrackingAuthorizationAsync();
                    if (status == ATTrackingManagerAuthorizationStatus.Authorized)
                    {
                        return GetIDFA();
                    }
                }
                else
                {
                    if (ASIdentifierManager.SharedManager.IsAdvertisingTrackingEnabled)
                    {
                        return GetIDFA();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"iOS IDFA error: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Checks if App Tracking Transparency is authorized on iOS.
        /// </summary>
        /// <returns>True if authorized/enabled, false otherwise.</returns>
        public async Task<bool> IsTrackingEnabledAsync()
        {
            try
            {
                if (NSProcessInfo.ProcessInfo.IsOperatingSystemAtLeastVersion(new NSOperatingSystemVersion(14, 0, 0)))
                {
                    var status = await ATTrackingManager.RequestTrackingAuthorizationAsync();
                    return status == ATTrackingManagerAuthorizationStatus.Authorized;
                }
                else
                {
                    return ASIdentifierManager.SharedManager.IsAdvertisingTrackingEnabled;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"iOS tracking status error: {ex.Message}");
                return false;
            }
        }

        private string? GetIDFA()
        {
            try
            {
                var idfa = ASIdentifierManager.SharedManager.AdvertisingIdentifier.AsString();

                if (!string.IsNullOrEmpty(idfa) &&
                    idfa != "00000000-0000-0000-0000-000000000000")
                {
                    return idfa;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"IDFA extraction error: {ex.Message}");
            }

            return null;
        }
    }
}