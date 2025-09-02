FacebookAppEvents.Mobile
<div align="center">
[![NuGet](https://img.shields.io/nuget/v/FacebookAppEvents.p Events shouldn't be this hard*

Get Started - Examples - Troubleshooting

</div>
The Problem
You want to track app events in Facebook. You install their SDK. It's 50MB, breaks your build, and requires 30 lines of setup code just to track a simple purchase. There's got to be a better way.

The Solution
This library does exactly one thing: sends Facebook App Events from your .NET MAUI app. No bloat, no complexity, no headaches. It handles the annoying parts (getting IDFA/GAID, privacy permissions) so you can focus on what matters.

What You Get
Works with iOS and Android MAUI apps

Handles advertising IDs automatically (IDFA on iOS, GAID on Android)

Respects user privacy (ATT on iOS 14+, LAT on Android)

Simple methods for common events (purchase, add to cart, login, etc.)

Fully customizable when you need it

Actually documented

Installation
bash
dotnet add package FacebookAppEvents.Mobile
Setup
First, get your Facebook App ID and Client Token from developers.facebook.com.

Android
Add this to AndroidManifest.xml:

xml
<uses-permission android:name="com.google.android.gms.permission.AD_ID" />
iOS
Add this to Info.plist:

xml
<key>NSUserTrackingUsageDescription</key>
<string>This helps us show you relevant ads and improve the app.</string>
Register the Services
In MauiProgram.cs:

csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    
#if ANDROID
    builder.Services.AddSingleton<IAdvertiserIdService, 
        FacebookAppEvents.Platforms.Android.AdvertiserIdService>();
#elif IOS
    builder.Services.AddSingleton<IAdvertiserIdService, 
        FacebookAppEvents.Platforms.iOS.AdvertiserIdService>();
#endif

    builder.Services.AddSingleton<FacebookAppEventSender>(provider =>
    {
        var httpClient = new HttpClient();
        var advertiserIdService = provider.GetRequiredService<IAdvertiserIdService>();
        return new FacebookAppEventSender(httpClient, "YOUR_APP_ID", "YOUR_CLIENT_TOKEN", advertiserIdService);
    });

    return builder.Build();
}
Examples
Track a Purchase
csharp
public async Task OnOrderCompleted(Order order)
{
    var items = order.Items.Select(item => new FacebookContentItem
    {
        Id = item.ProductId,
        Quantity = item.Quantity
    }).ToList();

    var purchaseEvent = FacebookAppEventFactory.CreatePurchaseEvent(
        items, 
        order.Total, 
        "USD"
    );

    await _eventSender.SendEventsAsync(purchaseEvent);
}
Track Add to Cart
csharp
var items = new List<FacebookContentItem>
{
    new() { Id = "product-123", Quantity = 1 }
};

var event = FacebookAppEventFactory.CreateAddToCartEvent(items);
await _eventSender.SendEventsAsync(event);
Track Screen Views
csharp
// In your page's OnAppearing or constructor
var screenEvent = FacebookAppEventFactory.CreateScreenViewEvent("ProductDetails");
await _eventSender.SendEventsAsync(screenEvent);
Track User Registration
csharp
var loginEvent = FacebookAppEventFactory.CreateLoginEvent();
await _eventSender.SendEventsAsync(loginEvent);
Custom Events
csharp
var customEvent = FacebookAppEventFactory.CreateCustomEvent(
    eventName: "video_completed",
    contentType: "media",
    contents: new List<FacebookContentItem>
    {
        new() { Id = "intro_video", Quantity = 1 }
    }
);

await _eventSender.SendEventsAsync(customEvent);
Send Multiple Events
csharp
// More efficient than sending one by one
await _eventSender.SendEventsAsync(screenEvent, addToCartEvent, purchaseEvent);
Don't Like Dependency Injection?
Fair enough. You can create everything manually:

csharp
IAdvertiserIdService advertiserService;
#if ANDROID
advertiserService = new FacebookAppEvents.Platforms.Android.AdvertiserIdService();
#elif IOS
advertiserService = new FacebookAppEvents.Platforms.iOS.AdvertiserIdService();
#endif

var sender = new FacebookAppEventSender(
    new HttpClient(), 
    "YOUR_APP_ID", 
    "YOUR_CLIENT_TOKEN", 
    advertiserService
);

var event = FacebookAppEventFactory.CreatePurchaseEvent(items, 99.99, "USD");
await sender.SendEventsAsync(event);
Privacy
This library respects user privacy:

iOS 14+: Shows the App Tracking Transparency dialog automatically

Android: Checks if user has limited ad tracking

Both: If users opt out, sends empty advertiser IDs and marks tracking as disabled

No sketchy stuff, no personal data without consent.

Troubleshooting
Events not showing up in Facebook?

Check your App ID and Client Token (classic mistake)

Events take 15-20 minutes to appear in Facebook's dashboard

Test on a real device, not simulator

iOS permission issues?

Make sure the ATT description is in Info.plist

Permission dialog only shows once per app install

Need iOS 14+ for ATT

Android advertising ID not working?

Verify AD_ID permission is in AndroidManifest.xml

Update Google Play Services on your test device

Library handles threading automatically (you're welcome)

API Reference
FacebookAppEventFactory Methods
Method	Purpose
CreatePurchaseEvent()	Track completed purchases
CreateAddToCartEvent()	Track items added to cart
CreateRemoveFromCartEvent()	Track items removed from cart
CreateScreenViewEvent()	Track page/screen views
CreateLoginEvent()	Track user registration/login
CreateSearchEvent()	Track search queries
CreateCustomEvent()	Create any custom event
FacebookAppEvent Properties
Property	Type	Description
EventName	string	Facebook event name (required)
EventId	string	Unique event identifier
FbContent	List<FacebookContentItem>	Items involved in event
FbContentType	string	Content type ("product", "screen", etc.)
ValueToSum	double?	Monetary value
FbCurrency	string	Currency code ("USD", "EUR", etc.)
Contributing
Found a bug? Want to add something? Cool, but let's keep it simple. This library intentionally does one thing well rather than trying to be everything to everyone.

Fork it

Create your feature branch (git checkout -b fix-something-broken)

Commit your changes (git commit -am 'Fix the thing')

Push to the branch (git push origin fix-something-broken)

Create a Pull Request

License
MIT License. Use it however you want.

Support
🐛 Bug Reports: Create an issue

💡 Feature Requests: Start a discussion

📖 Documentation: Check the XML docs in your IDE

Made with coffee and mild annoyance at Facebook's unnecessarily complex SDKs.