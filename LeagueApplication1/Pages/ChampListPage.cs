using Xamarin.Forms;
using RiotSharp;
using Acr.UserDialogs;
using System.Collections.ObjectModel;
using RiotSharp.StaticDataEndpoint;
using System.Threading.Tasks;
using System.Linq;

namespace LeagueApplication1
{
	class ChampListPage : ContentPage
	{
		ListView listView;
		ObservableCollection<ChampionStatic> champs;
		public ChampListPage()
		{
			Title = "Champions";
			var staticApi = App.staticApi;

			var imageTemplate = new DataTemplate(typeof(ImageCell));
			imageTemplate.SetBinding(TextCell.TextProperty, "Name");
			imageTemplate.SetBinding(ImageCell.ImageSourceProperty, "Image.Full");
			imageTemplate.SetBinding(TextCell.DetailProperty, "Title");

			NavigationPage.SetHasNavigationBar(this, true);
			listView = new ListView();
			listView.ItemTemplate = imageTemplate;

			var allChampions = App.staticApi.GetChampions(Region.euw, RiotSharp.StaticDataEndpoint.ChampionData.image);

			var tempChamps = allChampions.Champions.Values.ToList();
			var tempList2 = tempChamps.OrderBy(x => x.Name).ToList();

			foreach (var champ in tempList2)
			{
				champ.Image.Full = string.Format("http://ddragon.leagueoflegends.com/cdn/{0}/img/champion/{1}", App.appVersion, champ.Image.Full);
			}
			champs = new ObservableCollection<ChampionStatic>(tempList2);

			listView.ItemsSource = champs;

			var adView = new AdMobView { WidthRequest = 320, HeightRequest = 50 };

			var stackLayout = new StackLayout();

			stackLayout.Children.Add(listView);
			stackLayout.Children.Add(adView);

			Content = stackLayout;

			listView.ItemTapped += async (sender, e) =>
			{
				var myListView = (ListView)sender;
				var champion = (ChampionStatic)myListView.SelectedItem;
				UserDialogs.Instance.ShowLoading("Loading " + champion.Name, MaskType.Black);
				var champPlayed = staticApi.GetChampionAsync(Region.euw, champion.Id, RiotSharp.StaticDataEndpoint.ChampionData.all);
				await champPlayed;
				UserDialogs.Instance.HideLoading();
				await Navigation.PushAsync(new ChampionPage(champPlayed.Result));
			};
		}

	}
}

