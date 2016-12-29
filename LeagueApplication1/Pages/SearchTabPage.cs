using System;
using RiotSharp;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace LeagueApplication1
{
	public class SearchTabPage : ContentPage
	{
		CustomSearchBar searchBar;
		Picker picker;
		Region region = Region.euw;
		string icon = "http://avatar.leagueoflegends.com/summonerId/{0}/{1}.png";
		string iconReset = "http://avatar.leagueoflegends.com/summonerId/{0}/{1}.png";

		public SearchTabPage()
		{
			searchBar = new CustomSearchBar { Placeholder = "Search Summoner" };

			searchBar.SearchButtonPressed += async (sender, e) =>
			{
				string searchText = searchBar.Text.Replace(" ", "");
				searchBar.Text = "";
				searchBar.Unfocus();
				try
				{
					UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
					var summoner = await App.api.GetSummonerAsync(region, searchText);
					icon = string.Format(icon, summoner.Region, summoner.Id);
					var test = new RelativeLayoutPage(summoner, summoner.Region, this);
					UserDialogs.Instance.HideLoading();
					await Navigation.PushAsync(test);
				}
				catch
				{
					UserDialogs.Instance.HideLoading();
					await UserDialogs.Instance.AlertAsync("Try again", "Summoner not found!", "Okay");
				}
			};

			picker = new Picker();
			picker.Title = "Region";

			picker.Items.Add(Region.euw.ToString().ToUpper());
			picker.Items.Add(Region.eune.ToString().ToUpper());
			picker.Items.Add(Region.na.ToString().ToUpper());
			picker.Items.Add(Region.kr.ToString().ToUpper());
			picker.Items.Add(Region.br.ToString().ToUpper());
			picker.Items.Add(Region.lan.ToString().ToUpper());
			picker.Items.Add(Region.las.ToString().ToUpper());
			picker.Items.Add(Region.oce.ToString().ToUpper());
			picker.Items.Add(Region.ru.ToString().ToUpper());
			picker.Items.Add(Region.tr.ToString().ToUpper());

			picker.SelectedIndex = 0;

			picker.SelectedIndexChanged += (sender, args) =>
			{
				switch (picker.SelectedIndex)
				{
					case 0:
						region = Region.euw;
						break;
					case 1:
						region = Region.eune;
						break;
					case 2:
						region = Region.na;
						break;
					case 3:
						region = Region.kr;
						break;
					case 4:
						region = Region.br;
						break;
					case 5:
						region = Region.lan;
						break;
					case 6:
						region = Region.las;
						break;
					case 7:
						region = Region.oce;
						break;
					case 8:
						region = Region.ru;
						break;
					case 9:
						region = Region.tr;
						break;
				}
			};

			Content = new StackLayout
			{
				Children = {
					searchBar,
					picker
				}
			};
		}
	}
}

