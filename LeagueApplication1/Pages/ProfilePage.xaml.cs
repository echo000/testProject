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
 public partial class ProfilePage : ContentPage
	{

		public static RiotApi api = App.api;
		public static StaticRiotApi staticApi = App.staticApi;
		public static Region Region;
		public static Summoner Summoner;
		static ObservableCollection<GroupedSummoners> grouped { get; set; }
		public static List<Summoner> summoners;
		public static Dictionary<long, List<RiotSharp.LeagueEndpoint.League>> allLeagues;
		public static RiotSharp.LeagueEndpoint.League ranked5v5League;
		public static string div;
		static public FilterControl filterControl;
		public static ContentView currentGameView;

		public string skinString = "http://ddragon.leagueoflegends.com/cdn/img/champion/splash/{0}_{1}.jpg";
		static string icon = "http://avatar.leagueoflegends.com/summonerId/{0}/{1}.png";

		public ProfilePage(Summoner summoner, Region region)
		{
			InitializeComponent();
			nameText.BackgroundColor = Color.FromRgba(0, 0, 0, 0.6);
			this.Title = "Profile";
			Region = region;
			Summoner = summoner;
			filterControl = new FilterControl { Items = new List<string> { "Match History", "Current Game", "Statistics" } };
			filterControl.SelectedIndex = 0;
			contentViewFilter.Content = filterControl;

			nameText.Text = summoner.Name;

			summonerIcon.Source = string.Format(icon, region, summoner.Id);

			var frequentlyPlayed = Task.Run(async () =>
			{
				var matchList = await api.GetRecentGamesAsync(region, summoner.Id);
				var freqPlayed = matchList.GroupBy(q => q.ChampionId)
										  .OrderByDescending(gp => gp.Count())
										  .Select(g => g.Key)
										  .First();
				var championPlayedMost = staticApi.GetChampion(region, (int)freqPlayed, ChampionData.skins);
				freqPlayedChamp.Source = string.Format(skinString, championPlayedMost.Key, championPlayedMost.Skins[0].Num);
			});
			frequentlyPlayed.Wait();

			var MatchListTask = Task.Run(async () =>
			{
				var MatchList = await loadMatchHistory(summoner, region, this);
				contentVue.Content = MatchList;
			});
			MatchListTask.Wait();

			filterControl.SelectedIndexChanged += async (object sender, EventArgs e) =>
			{
				switch (filterControl.SelectedIndex)
				{
					case 0:
						UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
						var MatchList = await loadMatchHistory(summoner, region, this);
						contentVue.Content = MatchList;
						UserDialogs.Instance.HideLoading();
						break;
					case 1:
						UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
						currentGameView = await loadCurrentGameView(summoner, summoner.Region, this);
						contentVue.Content = currentGameView;
						UserDialogs.Instance.HideLoading();
						break;
					case 2:
						UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
						var scrollView = await loadPlayerStats(summoner, region);
						scrollView.WidthRequest = contentVue.Width;
						contentVue.Content = scrollView;
						UserDialogs.Instance.HideLoading();
						break;
				}
			};
		}

		public static async Task<ListView> loadMatchHistory(Summoner summoner, Region region, Page SearchPage)
		{
			var allItems = await staticApi.GetItemsAsync(region, ItemData.image);
			var allSummonerSpells = await staticApi.GetSummonerSpellsAsync(region, SummonerSpellData.image);

			var previousMatches = await summoner.GetRecentGamesAsync();
			var previousAll = new List<Previous>();
			for (int i = 0; i < previousMatches.Count; i++)
			{
				var gameType = App.gameTypeName(previousMatches[i].GameSubType);
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
						itemNameList.Add(string.Format("http://ddragon.leagueoflegends.com/cdn/{0}/img/item/{1}", App.appVersion,itemList[j].Image.Full));
					else
						itemNameList.Add("Empty.png");
				}

				var prev = new Previous(
					gameType,
					previousMatches[i].Level,
					previousMatches[i].Statistics.Win,
					App.FormatNumber(previousMatches[i].Statistics.GoldEarned),
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
			var listView = new ListView
			{
				ItemTemplate = imageTemplate,
				HasUnevenRows = true,
				ItemsSource = previousAll
			};

			listView.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
			{
				var myListView = (ListView)sender;
				var myItem = (Previous)myListView.SelectedItem;
				UserDialogs.Instance.ShowLoading("Loading Game", MaskType.Black);
				try
				{
					Debug.WriteLine(myItem.ThisGame.GameId);
					var match = await api.GetMatchAsync(Region, myItem.ThisGame.GameId);
					var previousPage = await PreviousGamePage.loadPrevious(match, myItem.ThisGame, Region, Summoner, myItem.ThisGame.FellowPlayers);
					UserDialogs.Instance.HideLoading();
					await SearchPage.Navigation.PushAsync(previousPage);
				}
				catch (Exception ex)
				{
					UserDialogs.Instance.HideLoading();
					Debug.WriteLine(ex);
					await UserDialogs.Instance.AlertAsync("Are you connected to the internet?\nTry again", "An Error Occured", "Okay");
				}
			};
			return listView;
		}
		public static async Task<ScrollView> loadPlayerStats(Summoner summoner, Region region)
		{
			var stats = await api.GetStatsSummariesAsync(region, summoner.Id);
			var scrollView = new ScrollView();
			var stack = new StackLayout();
			var statList = new List<string>();

			for (int i = 0; i < stats.Count; i++)
			{
				var gameStatType = App.getGameStatType(stats[i].PlayerStatSummaryType);
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

		//Loads the current game if there is one
		public static async Task<ContentView> loadCurrentGameView(Summoner summoner, Region region, Page SearchPage)
		{
			var gameView = new ContentView();

			var currentTemplate = new DataTemplate(typeof(ImageCell));
			currentTemplate.SetBinding(TextCell.TextProperty, "Name");
			currentTemplate.SetBinding(ImageCell.ImageSourceProperty, "Icon");
			currentTemplate.SetBinding(TextCell.DetailProperty, "Rank");
			try
			{
				var currentGame = await api.GetCurrentGameAsync(App.PlatformToRegion(region), summoner.Id);
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
					var myListView = (ListView)sender;
					var myItem = (CurrentGamePlayers)myListView.SelectedItem;
					try
					{
						UserDialogs.Instance.ShowLoading("Loading " + myItem.Name, MaskType.Black);
						var summonerAsync = await api.GetSummonerAsync(myItem.Region, myItem.Name);
						var page = SearchPage.Navigation.NavigationStack.Last();
						var test = new RelativeLayoutPage(summonerAsync, myItem.Region, SearchPage);
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
	}
}
