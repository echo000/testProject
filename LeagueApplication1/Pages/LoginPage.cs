using System;
using RiotSharp;
using Xamarin.Forms;

namespace LeagueApplication1
{
	public class LoginPage : ContentPage
	{
		CustomSearchBar searchBar;
		ICredentialsService storeService;
		Picker picker;
		Region region = Region.euw;

		public LoginPage()
		{
			var label = new Label { Text = "Search for your profile name" };
			storeService = DependencyService.Get<ICredentialsService>();

			searchBar = new CustomSearchBar { Placeholder = "Search Summoner" };

			searchBar.SearchButtonPressed += async (sender, e) =>
			{
				var summonerName = searchBar.Text.Replace(" ", "");
				var summoner = await App.api.GetSummonerAsync(region, summonerName);

				bool doCredentialsExist = storeService.DoCredentialsExist();
				if (!doCredentialsExist)
				{
					storeService.SaveCredentials(summoner.Name, summoner.Region.ToString(), summoner.Id.ToString());
				}

				Navigation.InsertPageBefore(new TabbedPages(summoner, summoner.Region), this);
				await Navigation.PopAsync();
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
					picker,
					label
				}
			};
		}
	}
}

