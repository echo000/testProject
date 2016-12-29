using System.Collections.ObjectModel;
using System.Threading.Tasks;
using RiotSharp;
using Xamarin.Forms;

namespace LeagueApplication1
{
	public class FavoritesMaster : ContentPage
	{
		public ListView ListView { get { return listview;} }
		public CustomSearchBar SearchBar { get { return searchBar; } }
		public Region Region { get { return region; } }

		CustomSearchBar searchBar;
		Region region = Region.euw;
		Picker picker;
		ListView listview;
		RiotApi api = App.api;
		string icon = "http://avatar.leagueoflegends.com/summonerId/{0}/{1}.png";
		string iconReset = "http://avatar.leagueoflegends.com/summonerId/{0}/{1}.png";
		ObservableCollection<GroupedSearchesAndFavorites> grouped { get; set; }

		public async Task getSummAsync()
		{
			grouped = new ObservableCollection<GroupedSearchesAndFavorites>();
			var searchesGroup = new GroupedSearchesAndFavorites { LongName = "Favorite", ShortName = "F" };

			for (int i = 0; i < App.FavoriteDatabase.GetItems().Count; i++)
			{
				if (App.FavoriteDatabase.GetItems()[i].Name != "")
				{
					var reg = regFromString(App.FavoriteDatabase.GetItems()[i].Region.ToLower());
					var sum = await api.GetSummonerAsync(reg, App.FavoriteDatabase.GetItems()[i].summonerID);
					if (sum.Name == App.FavoriteDatabase.GetItems()[i].Name)
						searchesGroup.Add(App.FavoriteDatabase.GetItems()[i]);
					else
					{
						App.FavoriteDatabase.DeleteItem(i + 1);
						icon = string.Format(icon, sum.Region, sum.Id);
						var newItem = new SearchesAndFavorites { Name = sum.Name, Region = sum.Region.ToString().ToUpper(), Icon = icon, summonerID = (int)sum.Id };
						App.FavoriteDatabase.SaveItem(newItem);
						searchesGroup.Add(newItem);
						icon = iconReset;
					}
				}
			}
		}

		public async Task Refresh()
		{
			grouped = new ObservableCollection<GroupedSearchesAndFavorites>();
			var searchesGroup = new GroupedSearchesAndFavorites { LongName = "Favourite", ShortName = "F" };
			for (int i = 0; i < App.FavoriteDatabase.GetItems().Count; i++)
			{
				if (App.FavoriteDatabase.GetItems()[i].Name != "")
				{
					searchesGroup.Add(App.FavoriteDatabase.GetItems()[i]);
				}
			}
			grouped.Add(searchesGroup);

			listview.ItemsSource = grouped;
			listview.IsGroupingEnabled = true;
			//listview.GroupDisplayBinding = new Binding("LongName");

			await getSummAsync();
		}

		public FavoritesMaster()
		{
			var recentTemplate = new DataTemplate(typeof(ImageCell));
			recentTemplate.SetBinding(TextCell.TextProperty, "Name");
			recentTemplate.SetBinding(ImageCell.ImageSourceProperty, "Icon");

			searchBar = new CustomSearchBar { Placeholder = "Search Summoner" };

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

			listview = new ListView
			{
				ItemTemplate = recentTemplate,
				SeparatorVisibility = SeparatorVisibility.None,
				HasUnevenRows = true,
				GroupHeaderTemplate = new DataTemplate(typeof(GroupHeader))
			};

			Title = "Test";
			Icon = "Star.png";

			if (App.FavoriteDatabase.GetItems().Count <= 5)
			{
				listview.HeightRequest = (App.FavoriteDatabase.GetItems().Count * 105) / 2;
			}
			else
			{
				listview.HeightRequest = (5 * 105) / 2;
			}

			var scrollView = new ScrollView
			{
				Content = new StackLayout
				{
					Children = {
						listview,
						new HtmlLabel { Text = "<b>League Summoners</b> isn't endorsed by Riot Games and doesn't reflect the views or opinions of Riot Games or anyone officially involved in producing or managing League of Legends.<br> League of Legends and Riot Games are trademarks or registered trademarks of Riot Games, Inc. League of Legends © Riot Games, Inc.", FontSize = 12 }
					}
				}
			};

			Padding = new Thickness(0, 20, 0, 0);
			Content = new StackLayout
			{
				Children = {
					new Image{ Source = "Sona_Splash_6.jpg", HeightRequest = 100, Aspect = Aspect.AspectFill, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand },
					searchBar,
					picker,
					scrollView
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

