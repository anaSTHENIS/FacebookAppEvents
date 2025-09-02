using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FacebookAppEvents.src
{
    /// <summary>
    /// Interface for retrieving platform-specific advertising identifiers.
    /// </summary>
    public interface IAdvertiserIdService
    {
        /// <summary>
        /// Gets the advertising identifier for the current platform.
        /// Returns null if unavailable or user has opted out.
        /// </summary>
        /// <returns>The advertising ID (IDFA on iOS, GAID on Android) or null if unavailable.</returns>
        Task<string?> GetAdvertiserIdAsync();

        /// <summary>
        /// Determines if advertising tracking is enabled by the user.
        /// </summary>
        /// <returns>True if tracking is enabled, false otherwise.</returns>
        Task<bool> IsTrackingEnabledAsync();
    }
}
