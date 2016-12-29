using Xamarin.Forms;
using RiotSharp;
using Acr.UserDialogs;

namespace LeagueApplication1
{
	class ChampListPage : ContentPage
	{
		ChampionListView listView;
		public ChampListPage()
		{
			Title = "Champions";
			var staticApi = App.staticApi;

			NavigationPage.SetHasNavigationBar(this, true);
			listView = new ChampionListView();
			//listView.SeparatorVisibility = SeparatorVisibility.None;

			var adView = new AdMobView { WidthRequest = 320, HeightRequest = 50 };

			var stackLayout = new StackLayout();

			stackLayout.Children.Add(listView);
			stackLayout.Children.Add(adView);

			Content = stackLayout;

			listView.ItemTapped += async (sender, e) =>
			{
				var myListView = (ListView)sender;
				var champion = (Champ)myListView.SelectedItem;
				UserDialogs.Instance.ShowLoading("Loading " + champion.Name, MaskType.Black);
				var champPlayed = staticApi.GetChampionAsync(Region.euw, champion.champID, RiotSharp.StaticDataEndpoint.ChampionData.all);
				await champPlayed;
				UserDialogs.Instance.HideLoading();
				await Navigation.PushAsync(new ChampionPage(champPlayed.Result));
			};
		}

	}
}

