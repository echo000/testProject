using System;
using Xamarin.Forms;
using RiotSharp;
using System.Collections.Generic;
using RiotSharp.SummonerEndpoint;
using RiotSharp.GameEndpoint.Enums;
using System.Linq;
using RiotSharp.StatsEndpoint.Enums;
using RiotSharp.StaticDataEndpoint;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Acr.UserDialogs;
using System.Diagnostics;

namespace LeagueApplication1
{
	public class DownloadImages : ContentPage
	{
		static string icon = "http://avatar.leagueoflegends.com/summonerId/{0}/{1}.png";
		static string iconReset = "http://avatar.leagueoflegends.com/summonerId/{0}/{1}.png";
		static public FilterControl memeTest;
		static ObservableCollection<GroupedSummoners> grouped { get; set; }
		static ToolbarItem infoIcon;
		static Grid grid;
		static ContentView contentVue;
		public static Page SearchPage;
		public static List<Summoner> summoners;
		public static ContentView currentGameView;
		public static RiotApi api = App.api;
		public static StaticRiotApi staticApi = App.staticApi;

		public static Dictionary<long, List<RiotSharp.LeagueEndpoint.League>>  allLeagues;
		public static RiotSharp.LeagueEndpoint.League ranked5v5League;
		public static string div;

		static Platform PlatformToRegion(Region region)
		{
			switch (region)
			{
				case Region.euw:
					return Platform.EUW1;
				case Region.eune:
					return Platform.EUN1;
				case Region.br:
					return Platform.BR1;
				case Region.kr:
					return Platform.KR;
				case Region.lan:
					return Platform.LA1;
				case Region.las:
					return Platform.LA1;
				case Region.na:
					return Platform.NA1;
				case Region.oce:
					return Platform.OC1;
				case Region.ru:
					return Platform.RU;
				case Region.tr:
					return Platform.TR1;
			}
			return Platform.EUW1;
		}

		static SummonerSpell GetSummonerSpellFromId(int id)
		{
			switch (id)
			{
				case 1:
					return SummonerSpell.Cleanse;
				case 2:
					return SummonerSpell.Clairvoyance;
				case 3:
					return SummonerSpell.Exhaust;
				case 4:
					return SummonerSpell.Flash;
				case 6:
					return SummonerSpell.Ghost;
				case 7:
					return SummonerSpell.Heal;
				case 11:
					return SummonerSpell.Smite;
				case 12:
					return SummonerSpell.Teleport;
				case 13:
					return SummonerSpell.Clarity;
				case 14:
					return SummonerSpell.Ignite;
				case 21:
					return SummonerSpell.Barrier;
				case 30:
					return SummonerSpell.PoroRecall;
				case 31:
					return SummonerSpell.PoroToss;
				case 32:
					return SummonerSpell.Mark;
			}
			return SummonerSpell.Ghost;
		}

		public DownloadImages(Summoner summoner)
		{
			NavigationPage.SetHasNavigationBar(this, true);
			ToolbarItems.Add(infoIcon);
			Title = summoner.Name;
			infoIcon.Clicked += (sender, e) =>
			{
				if ((App.FavoriteDatabase.GetItems().Where(item => item.Name == summoner.Name && item.Region == summoner.Region.ToString().ToUpper()).ToList().Count > 0))
				{
					UserDialogs.Instance.AlertAsync(summoner.Name + " is already in your favorites, do you really love them that much? ;)","Error", "Okay");
				}
				else
				{
					UserDialogs.Instance.AlertAsync( summoner.Name + " has been added to your favorites", "Favorite Added", "Okay");
					App.FavoriteDatabase.SaveItem(new SearchesAndFavorites { Name = summoner.Name, Region = summoner.Region.ToString().ToUpper(), Icon = icon, summonerID = (int)summoner.Id });
				}
			};

			var adView = new AdMobView { WidthRequest = 320, HeightRequest = 50 };

			var stackLayout = new StackLayout
			{
				Children = {
					grid,
					memeTest,
					contentVue,
					adView
				}
			};

			Content = stackLayout;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			icon = iconReset;
		}

		public static async Task<DownloadImages> loadPage(Summoner summoner, Region region, Page searchPage)
		{
			SearchPage = searchPage;
			infoIcon = new ToolbarItem { Text = "+" };
			icon = string.Format(icon, summoner.Region, summoner.Id);

			if (!(App.RecentDatabase.GetItems().Where(item => item.Name == summoner.Name && item.Region == summoner.Region.ToString().ToUpper()).ToList().Count > 0))
			{
				App.RecentDatabase.SaveItem(new SearchesAndFavorites { Name = summoner.Name, Region = summoner.Region.ToString().ToUpper(), Icon = icon, summonerID = (int)summoner.Id });
			}

			if (App.RecentDatabase.GetItems().Count > 10)
			{
				App.RecentDatabase.DeleteItem(1);
			}

			memeTest = new FilterControl { Items = new List<string> { "Match History", "Current Game", "Statistics" } };
			memeTest.SelectedIndex = 0;
			var webImage = new Image { Aspect = Aspect.AspectFit };
			var rankedImage = new Image { WidthRequest = 50, HeightRequest = 50 };

			webImage.Source = ImageSource.FromUri(new Uri(icon));
			var rankedInfo = new Label();

			try
			{
				allLeagues = api.GetLeagues(summoner.Region, new List<int> { (int)summoner.Id });
				ranked5v5League = allLeagues[summoner.Id].Single(x => x.Queue == Queue.RankedSolo5x5);
				div = ranked5v5League.Entries.Where(x => x.PlayerOrTeamId == summoner.Id.ToString()).Select(x => x.Division).Single();
			}
			catch
			{
				ranked5v5League = null;
				div = null;
				allLeagues = null;
			}

			if (ranked5v5League == null)
			{
				rankedImage.Source = ImageSource.FromFile("Unranked.png");
				rankedInfo.Text = "Unranked";
			}
			else {
				rankedImage.Source = ImageSource.FromFile(ranked5v5League.Tier + ".png");
				if (ranked5v5League.Tier == RiotSharp.LeagueEndpoint.Enums.Tier.Challenger || ranked5v5League.Tier == RiotSharp.LeagueEndpoint.Enums.Tier.Master)
				{
					rankedInfo.Text = ranked5v5League.Tier.ToString();
				}
				else {
					rankedInfo.Text = ranked5v5League.Tier + " " + div;
				}
			}

			grid = new Grid
			{
				RowDefinitions =
				{
					new RowDefinition { Height = new GridLength(50, GridUnitType.Absolute)}
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute)},
					new ColumnDefinition { Width = GridLength.Auto},
					new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute)},
					new ColumnDefinition { Width = GridLength.Auto}
				}
			};

			contentVue = new ContentView();

			grid.Children.Add(webImage, 0, 0);
			grid.Children.Add(new Label
			{
				Text = summoner.Name + Environment.NewLine + "Level: " + summoner.Level
			}, 1, 0);
			grid.Children.Add(rankedImage, 2, 0);
			grid.Children.Add(rankedInfo, 3, 0);

			var MatchList = await loadMatchHistory(summoner, region);
			contentVue.Content = MatchList;

			memeTest.SelectedIndexChanged += async (object sender, EventArgs e) =>
			{
				switch (memeTest.SelectedIndex)
				{
					case 0:
						UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
						MatchList = await loadMatchHistory(summoner, region);
						contentVue.Content = MatchList;
						UserDialogs.Instance.HideLoading();
						break;
					case 1:
						UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
						currentGameView = await loadCurrentGameView(summoner, summoner.Region);
						contentVue.Content = currentGameView;
						UserDialogs.Instance.HideLoading();
						break;
					case 2:
						UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
						var scrollView = await loadPlayerStats(summoner, region);
						contentVue.Content = scrollView;
						UserDialogs.Instance.HideLoading();
						break;
				}
			};
			return new DownloadImages(summoner);
		}

		//Loads the match history
		public static async Task<ListView> loadMatchHistory(Summoner summoner, Region region)
		{
			var allItems = await staticApi.GetItemsAsync(region, ItemData.image);
			var allSummonerSpells = await staticApi.GetSummonerSpellsAsync(region, SummonerSpellData.image);

			var previousMatches = await summoner.GetRecentGamesAsync();
			var previousAll = new List<Previous>();
			for (int i = 0; i < previousMatches.Count; i++)
			{
				var gameType = gameTypeName(previousMatches[i].GameSubType);
				var champPlayed = await staticApi.GetChampionAsync(region, previousMatches[i].ChampionId, ChampionData.image);
				var summonerSpellList = new List<string>
				{
					allSummonerSpells.SummonerSpells.FirstOrDefault(y => y.Value.Id == previousMatches[i].SummonerSpell1).Value.Image.Full,
					allSummonerSpells.SummonerSpells.FirstOrDefault(y => y.Value.Id == previousMatches[i].SummonerSpell2).Value.Image.Full
				};
				var itemList = new List<ItemStatic>
				{
					allItems.Items.FirstOrDefault((x) => x.Value.Id == previousMatches[i].Statistics.Item0).Value,
					allItems.Items.FirstOrDefault((x) => x.Value.Id == previousMatches[i].Statistics.Item1).Value,
					allItems.Items.FirstOrDefault((x) => x.Value.Id == previousMatches[i].Statistics.Item2).Value,
					allItems.Items.FirstOrDefault((x) => x.Value.Id == previousMatches[i].Statistics.Item3).Value,
					allItems.Items.FirstOrDefault((x) => x.Value.Id == previousMatches[i].Statistics.Item4).Value,
					allItems.Items.FirstOrDefault((x) => x.Value.Id == previousMatches[i].Statistics.Item5).Value,
					allItems.Items.FirstOrDefault((x) => x.Value.Id == previousMatches[i].Statistics.Item6).Value
				};
				var itemNameList = new List<string>();
				for (int j = 0; j < itemList.Count; j++)
				{
					if (itemList[j] != null)
						itemNameList.Add(itemList[j].Image.Full);
					else
						itemNameList.Add("Empty.png");
				}

				var prev = new Previous(
					gameType,
					previousMatches[i].Level,
					previousMatches[i].Statistics.Win,
					FormatNumber(previousMatches[i].Statistics.GoldEarned),
					previousMatches[i].Statistics.MinionsKilled.ToString(),
					itemNameList,
					summonerSpellList,
					champPlayed.Image.Full,
					previousMatches[i].CreateDate.ToString("g"),
					previousMatches[i].Statistics.ChampionsKilled + "/" + previousMatches[i].Statistics.NumDeaths + "/" + previousMatches[i].Statistics.Assists,
					previousMatches[i]
				);
				previousAll.Add(prev);
			}

			var imageTemplate = new DataTemplate(typeof(PreviousGameCell));

			var MatchList = new ListView
			{
				ItemTemplate = imageTemplate,
				ItemsSource = previousAll,
				HasUnevenRows = true
			};

			MatchList.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
			{
				var myListView = (ListView)sender;
				var myItem = (Previous)myListView.SelectedItem;
				UserDialogs.Instance.ShowLoading("Loading Game", MaskType.Black);
				try
				{
					Debug.WriteLine(myItem.ThisGame.GameId);
					await Task.Delay(1000);
					var match = await api.GetMatchAsync(region, myItem.ThisGame.GameId);
					var previousPage = await PreviousGamePage.loadPrevious(match, myItem.ThisGame, region, summoner, myItem.ThisGame.FellowPlayers);
					UserDialogs.Instance.HideLoading();
					await SearchPage.Navigation.PushAsync(previousPage);
				}
				catch(Exception ex)
				{
					UserDialogs.Instance.HideLoading();
					Debug.WriteLine(ex);
					await UserDialogs.Instance.AlertAsync("Are you connected to the internet?\nTry again", "An Error Occured", "Okay");
				}
			};

			return MatchList;
		}

		//Takes the game sub type and returns the name as a string
		public static string gameTypeName(GameSubType gamesubType)
		{
			switch (gamesubType)
			{
				case GameSubType.None:
					return "Custom Game";
				case GameSubType.Normal:
					return "Normal";
				case GameSubType.Normal3x3:
					return "Normal 3v3";
				case GameSubType.OdinUnranked:
					return "Dominion";
				case GameSubType.AramUnranked5x5:
					return "ARAM";
				case GameSubType.Bot:
					return "Co-op vs AI";
				case GameSubType.Bot3x3:
					return "Co-op vs AI 3v3";
				case GameSubType.RankedSolo5x5:
					return "Ranked Solo";
				case GameSubType.RankedTeam3x3:
					return "Ranked Teams 3v3";
				case GameSubType.RankedTeam5x5:
					return "Ranked Teams 5v5";
				case GameSubType.OneForAll5x5:
					return "One For All";
				case GameSubType.FirstBlood1x1:
					return "Snowdown Showdown 1v1";
				case GameSubType.FirstBlood2x2:
					return "Snowdown Showdown 2v2";
				case GameSubType.SR_6x6:
					return "Summoners Rift 6v6";
				case GameSubType.TeamBuilder5x5:
					return "Team Builder 5v5";
				case GameSubType.URF:
					return "Ultra Rapid Fire";
				case GameSubType.URFBots:
					return "URF vs AI";
				case GameSubType.NightmareBot:
					return "Nightmate Mode";
				case GameSubType.Ascension:
					return "Ascension";
				case GameSubType.Hexakill:
					return "Hexakill";
				case GameSubType.KingPoro:
					return "Poro King";
				case GameSubType.CounterPick:
					return "Nemesis Draft";
				case GameSubType.Bilgewater:
					return "Black Market Brawlers";
				case GameSubType.NexusSiege:
					return "Nexus Siege";
			}
			return "";
		}


		//Loads the player Stats
		public static async Task<ScrollView> loadPlayerStats(Summoner summoner, Region region)
		{
			var stats = await api.GetStatsSummariesAsync(region, summoner.Id);
			var scrollView = new ScrollView();
			var stack = new StackLayout();
			var statList = new List<string>();

			for (int i = 0; i < stats.Count; i++)
			{
				var gameStatType = getGameStatType(stats[i].PlayerStatSummaryType);
				statList.Add(gameStatType);
				statList.Add("\tKills: " + stats[i].AggregatedStats.TotalChampionKills);
				statList.Add("\tAssists: " + stats[i].AggregatedStats.TotalAssists);
				statList.Add("\tMinions Killed: " + stats[i].AggregatedStats.TotalMinionKills);
				statList.Add("\tNeutral Minions Killed: " + stats[i].AggregatedStats.TotalNeutralMinionsKilled);
				statList.Add("\tTurret Kills: " + stats[i].AggregatedStats.TotalTurretsKilled);
			}
			for (int i = 0; i < statList.Count; i++)
			{
				stack.Children.Add(new Label { Text = statList[i] });
			}
			scrollView.Content = stack;
			return scrollView;
		}

		//Gets the game sub type for the stats
		public static string getGameStatType(PlayerStatsSummaryType stats)
		{
			switch (stats)
			{
				case PlayerStatsSummaryType.Unranked:
					return "Normal";
				case PlayerStatsSummaryType.Unranked3x3:
					return "Unranked 3v3";
				case PlayerStatsSummaryType.OdinUnranked:
					return "Dominion";
				case PlayerStatsSummaryType.AramUnranked5x5:
					return "ARAM";
				case PlayerStatsSummaryType.CoopVsAI:
					return "Co-op vs AI";
				case PlayerStatsSummaryType.CoopVsAI3x3:
					return "Co-op vs AI 3v3";
				case PlayerStatsSummaryType.RankedSolo5x5:
					return "Ranked";
				case PlayerStatsSummaryType.RankedPremade5x5:
					return "Ranked Premade 5v5";
				case PlayerStatsSummaryType.RankedPremade3x3:
					return "Ranked Premade 3v3";
				case PlayerStatsSummaryType.RankedTeam3x3:
					return "Ranked Teams 3v3";
				case PlayerStatsSummaryType.RankedTeam5x5:
					return "Ranked Teams 5v5";
				case PlayerStatsSummaryType.OneForAll5x5:
					return "One For All";
				case PlayerStatsSummaryType.FirstBlood1x1:
					return "Snowdown Showdown 1v1";
				case PlayerStatsSummaryType.FirstBlood2x2:
					return "Snowdown Showdown 2v2";
				case PlayerStatsSummaryType.SummonersRift6x6:
					return "Summoners Rift Hexakill";
				case PlayerStatsSummaryType.CAP5x5:
					return "Team Builder";
				case PlayerStatsSummaryType.URF:
					return "Ultra Rapid Fire";
				case PlayerStatsSummaryType.URFBots:
					return "Ultra Rapid Fire Vs AI";
				case PlayerStatsSummaryType.NightmareBot:
					return "Nightmate Mode";
				case PlayerStatsSummaryType.Ascension:
					return "Ascension";
				case PlayerStatsSummaryType.Hexakill:
					return "Hexakill";
				case PlayerStatsSummaryType.KingPoro:
					return "Legend of the Poro King";
				case PlayerStatsSummaryType.CounterPick:
					return "Nemesis Draft";
				case PlayerStatsSummaryType.Bilgewater:
					return "Black Market Brawlers";
				case PlayerStatsSummaryType.Siege:
					return "Nexus Siege";
			}
			return "";
		}

		//Loads the current game if there is one
		public static async Task<ContentView> loadCurrentGameView(Summoner summoner, Region region)
		{
			var gameView = new ContentView();

			var currentTemplate = new DataTemplate(typeof(ImageCell));
			currentTemplate.SetBinding(TextCell.TextProperty, "Name");
			currentTemplate.SetBinding(ImageCell.ImageSourceProperty, "Icon");
			currentTemplate.SetBinding(TextCell.DetailProperty, "Rank");
			try
			{
				var currentGame = await api.GetCurrentGameAsync(PlatformToRegion(region), summoner.Id);
				var currentGameStack = new StackLayout();
				grouped = new ObservableCollection<GroupedSummoners>();
				var blueTeam = new GroupedSummoners { LongName = "Blue Team", ShortName = "B" };
				var redTeam = new GroupedSummoners { LongName = "Red Team", ShortName = "R" };

				var list = new ListView();
				var idList = new List<string>();
				summoners = new List<Summoner>();
				for (int i = 0; i < currentGame.Participants.Count; i++)
				{
					idList.Add(currentGame.Participants[i].SummonerName);
				}
				summoners = await api.GetSummonersAsync(region, idList);

				for (int i = 0; i < currentGame.Participants.Count; i++)
				{
					for (int j = 0; j < summoners.Count; j++)
					{
						if (currentGame.Participants[i].SummonerName == summoners[j].Name)
						{
							var champPlayed = await staticApi.GetChampionAsync(region, (int)currentGame.Participants[i].ChampionId, ChampionData.image);
							try
							{
								allLeagues = await api.GetLeaguesAsync(region, new List<int> { (int)summoners[j].Id });
								ranked5v5League = allLeagues[summoners[j].Id].Single(x => x.Queue == Queue.RankedSolo5x5);
								div = ranked5v5League.Entries.Where(x => x.PlayerOrTeamId == summoners[j].Id.ToString()).Select(x => x.Division).Single();

								if (currentGame.Participants[i].TeamId == 100)
								{
									blueTeam.Add(new CurrentGamePlayers(summoners[j].Name, ranked5v5League.Tier + " " + div, (int)currentGame.Participants[i].ChampionId, champPlayed.Image.Full, summoners[i].Region));
								}
								else if (currentGame.Participants[i].TeamId == 200)
								{
									redTeam.Add(new CurrentGamePlayers(summoners[j].Name, ranked5v5League.Tier + " " + div, (int)currentGame.Participants[i].ChampionId, champPlayed.Image.Full, summoners[i].Region));
								}
							}
							catch
							{
								if (currentGame.Participants[i].TeamId == 100)
								{
									blueTeam.Add(new CurrentGamePlayers(summoners[j].Name, "Level " + summoners[i].Level, (int)currentGame.Participants[i].ChampionId, champPlayed.Image.Full, summoners[i].Region));
								}
								else if (currentGame.Participants[i].TeamId == 200)
								{
									redTeam.Add(new CurrentGamePlayers(summoners[j].Name, "Level " + summoners[i].Level, (int)currentGame.Participants[i].ChampionId, champPlayed.Image.Full, summoners[i].Region));
								}
							}
						}
					}
				}
				grouped.Add(blueTeam);
				grouped.Add(redTeam);

				list.ItemsSource = grouped;
				list.IsGroupingEnabled = true;
				list.GroupDisplayBinding = new Binding("LongName");
				list.ItemTemplate = currentTemplate;

				list.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
				{
					icon = iconReset;
					var myListView = (ListView)sender;
					var myItem = (CurrentGamePlayers)myListView.SelectedItem;
					try
					{
						UserDialogs.Instance.ShowLoading("Loading " + myItem.Name, MaskType.Black);
						var summonerAsync = await api.GetSummonerAsync(myItem.Region, myItem.Name);
						var page = SearchPage.Navigation.NavigationStack.Last();
						var test = await loadPage(summonerAsync, myItem.Region, SearchPage);
						UserDialogs.Instance.HideLoading();
						await SearchPage.Navigation.PushAsync(test);
						SearchPage.Navigation.RemovePage(page);
					}
					catch
					{
						UserDialogs.Instance.Alert("Whoops", "Something went wrong", "Okay");
					}
				};
				currentGameStack.Children.Add(
					new Label { Text = currentGame.GameMode.ToString() }
				);
				currentGameStack.Children.Add(list);
				gameView.Content = currentGameStack;
			}
			catch
			{
				gameView.Content = new Label { Text = summoner.Name + " is not in an active game. Please try again later if the summoner is currently in game." };
			}

			gameView.HeightRequest = 425;
			return gameView;
		}

		static string FormatNumber(int num)
		{
			if (num >= 100000)
				return FormatNumber(num / 1000) + "K";
			if (num >= 10000)
			{
				return (num / 1000D).ToString("0.#") + "K";
			}
			return num.ToString("#,0");
		}
	}

}