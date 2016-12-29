using Xamarin.Forms;
using System.Collections.ObjectModel;
using TwitchLib.Models.Static;
using System.Diagnostics;

namespace LeagueApplication1
{
	public class StreamPage : ContentPage
	{
		static TwitchApi twitchApi = App.twitchApi;
		FavoritesMaster fm;
		ObservableCollection<streamer> Channels;
		ListView StreamList = new ListView();

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			Channels = new ObservableCollection<streamer>();
			var games = await twitchApi.GetStream("streams?game=League+of+Legends", TwitchLib.Controllers.TwitchController.RequestType.Kraken);
			var streams = games.Streams;
			foreach (var stream in streams)
			{
				Channels.Add(new streamer(stream.Channel.DisplayName, stream.Preview.Large, stream.Viewers.ToString() + " viewers", stream.Channel.Status, stream));
			}

			StreamList.ItemsSource = Channels;

			SetDynamicResource(StyleProperty, "backgroundColor");
			((App)Application.Current).ResumeAtTodoId = -1;
			if (fm != null)
			{
				await fm.Refresh();
			}
		}

		public StreamPage()
		{
			StreamList.ItemTemplate = new DataTemplate(typeof(TwitchStreamCell));
			StreamList.SeparatorVisibility = SeparatorVisibility.None;
			StreamList.HasUnevenRows = true;

			StreamList.ItemSelected += async (sender, e) =>
			{
				var selected = e.SelectedItem as streamer;
				var streamEmbedLink = string.Format("https://twitch.tv/{0}/embed", selected.Stream.Channel.Name);
				var webview = new WebView
				{
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Source = new UrlWebViewSource
					{
						Url = streamEmbedLink
					}
				};
				var contentPage = new ContentPage
				{
					Content = new StackLayout
					{
						Children = { webview }
					},
					Title = selected.Name
				};
				await Navigation.PushAsync(contentPage);
			};

			NavigationPage.SetHasNavigationBar(this, true);
			Title = "Top Streams";

			var adView = new AdMobView { WidthRequest = 320, HeightRequest = 50 };

			var stacklayout = new StackLayout
			{
				Children = {
					StreamList,
					adView
					}
			};

			Content = stacklayout;
		}
	}
	class streamer
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public string Views { get; }
		public string preview { get; set;}
		public string Status { get; set; }
		public Stream Stream { get; set; }

		public streamer(string name, string image, string views,string status, Stream stream)
		{
			Name = name;
			Url = image;
			this.Stream = stream;
		}
	}
}