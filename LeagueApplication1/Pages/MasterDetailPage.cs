using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using RiotSharp;
using Xamarin.Forms;

namespace LeagueApplication1
{
	public class MasterPage : MasterDetailPage
	{
		RiotApi api = App.api;
		FavoritesMaster favMaster;
		string icon = "http://avatar.leagueoflegends.com/summonerId/{0}/{1}.png";
		string iconReset = "http://avatar.leagueoflegends.com/summonerId/{0}/{1}.png";

		public MasterPage()
		{
			favMaster = new FavoritesMaster();
			Master = favMaster;
			BackgroundColor = Color.FromRgb(36, 36, 36);
			Detail = new NavigationPage(new SearchPage(favMaster));

			favMaster.ListView.ItemSelected += async (sender, e) =>
			{
				var item = e.SelectedItem as SearchesAndFavorites;
				if (item != null)
				{
					try
					{
						UserDialogs.Instance.ShowLoading("Loading " + item.Name, MaskType.Black);
						Region region = regFromString(item.Region.ToLower());
						var summoner = await api.GetSummonerAsync(region, item.summonerID);
						var test = new RelativeLayoutPage(summoner, region, Detail);
						UserDialogs.Instance.HideLoading();
						IsPresented = false;
						await Detail.Navigation.PushAsync(test);
					}
					catch
					{
						UserDialogs.Instance.HideLoading();
						await UserDialogs.Instance.AlertAsync("Couldn't get that summoner.\n Are you connected to the internet?", "Error", "Okay");
					}
				}
			};
			favMaster.SearchBar.SearchButtonPressed += async (sender, e) =>
			{
				string searchText = favMaster.SearchBar.Text.Replace(" ", "");
				favMaster.SearchBar.Text = "";
				favMaster.SearchBar.Unfocus();
				try
				{
					UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
					var summoner = await api.GetSummonerAsync(favMaster.Region, searchText);
					icon = string.Format(icon, summoner.Region, summoner.Id);
					var test = new RelativeLayoutPage(summoner, summoner.Region, Detail);
					UserDialogs.Instance.HideLoading();
					IsPresented = false;
					await Detail.Navigation.PushAsync(test);
				}
				catch
				{
					UserDialogs.Instance.HideLoading();
					await UserDialogs.Instance.AlertAsync("Try again", "Summoner not found!", "Okay");
				}
			};

		}
		public Region regFromString(string regionName)
		{
			switch (regionName)
			{
				case "euw":
					return Region.euw;
				case "eune":
					return Region.eune;
				case "na":
					return Region.na;
				case "kr":
					return Region.kr;
				case "br":
					return Region.br;
				case "lan":
					return Region.lan;
				case "las":
					return Region.las;
				case "oce":
					return Region.oce;
				case "ru":
					return Region.ru;
				case "tr":
					return Region.tr;
			}
			return Region.global;
		}
	}
}
