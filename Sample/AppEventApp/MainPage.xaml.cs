using Plugin.Maui.FacebookAppEvents.Events;
using Plugin.Maui.FacebookAppEvents.Models;

namespace AppEventApp
{
    public partial class MainPage : ContentPage
    {
        private int _eventCounter = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnScreenViewClicked(object sender, EventArgs e)
        {
            try
            {
                // Using the new static fire-and-forget method
                FacebookAppEventSender.SendEvents(
                    FacebookAppEventFactory.CreateScreenViewEvent(nameof(MainPage))
                );

                UpdateStatus("Screen view event sent!");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}");
            }
        }

        private void OnPurchaseClicked(object sender, EventArgs e)
        {
            try
            {
                var items = new List<FacebookContentItem>
            {
                new() { Id = "product-123", Quantity = 2, ItemPrice = 29.99 },
                new() { Id = "product-456", Quantity = 1, ItemPrice = 49.99 }
            };

                FacebookAppEventSender.SendEvents(
                    FacebookAppEventFactory.CreatePurchaseEvent(items, 109.97, "USD")
                );

                UpdateStatus("Purchase event sent! Total: $109.97");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}");
            }
        }

        private void OnAddToCartClicked(object sender, EventArgs e)
        {
            try
            {
                var items = new List<FacebookContentItem>
            {
                new() { Id = "product-789", Quantity = 1, ItemPrice = 19.99 }
            };

                FacebookAppEventSender.SendEvents(
                    FacebookAppEventFactory.CreateAddToCartEvent(items)
                );

                UpdateStatus("Add to cart event sent!");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}");
            }
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                FacebookAppEventSender.SendEvents(
                    FacebookAppEventFactory.CreateLoginEvent()
                );

                UpdateStatus("Login event sent!");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}");
            }
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            try
            {
                FacebookAppEventSender.SendEvents(
                    FacebookAppEventFactory.CreateSearchEvent("maui facebook events")
                );

                UpdateStatus("Search event sent!");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}");
            }
        }

        private void OnCustomEventClicked(object sender, EventArgs e)
        {
            try
            {
                FacebookAppEventSender.SendEvents(
                    FacebookAppEventFactory.CreateCustomEvent(
                        eventName: "video_completed",
                        contentType: "media",
                        contents: new List<FacebookContentItem>
                        {
                        new() { Id = "intro_video", Quantity = 1 }
                        }
                    )
                );

                UpdateStatus("Custom event sent!");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}");
            }
        }

        private void OnMultipleEventsClicked(object sender, EventArgs e)
        {
            try
            {
                var screenEvent = FacebookAppEventFactory.CreateScreenViewEvent("MultipleEventsDemo");
                var searchEvent = FacebookAppEventFactory.CreateSearchEvent("multiple events test");
                var loginEvent = FacebookAppEventFactory.CreateLoginEvent();

                FacebookAppEventSender.SendEvents(screenEvent, searchEvent, loginEvent);

                UpdateStatus("Multiple events sent!");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error: {ex.Message}");
            }
        }

        private void UpdateStatus(string message)
        {
            _eventCounter++;
            StatusLabel.Text = $"[{_eventCounter}] {message}";
        }
    }
}
