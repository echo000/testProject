using Xamarin.Forms;
using RiotSharp;
using System.Threading.Tasks;
using RiotSharp.StaticDataEndpoint;
using RiotSharp.MatchEndpoint;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RiotSharp.SummonerEndpoint;
using RiotSharp.GameEndpoint;

namespace LeagueApplication1
{
	public class PreviousGamePage : ContentPage
	{
		ListView listView;
		static ObservableCollection<GroupedPrevious> grouped { get; set; }

		public PreviousGamePage(ObservableCollection<GroupedPrevious> grouped)
		{
			Title = "Previous Game";
			var imageTemplate = new DataTemplate(typeof(ExpandedGameCell));
			listView = new ListView
			{
				IsGroupingEnabled = true,
				ItemTemplate = imageTemplate,
				ItemsSource = grouped,
				HasUnevenRows = true,
				GroupDisplayBinding = new Binding("LongName")
			};
			var adView = new AdMobView { WidthRequest = 320, HeightRequest = 50 };
			var stackLayout = new StackLayout
			{
				Children = {
					listView,
					adView
				}
			};
			Content = stackLayout;
		}

		public static async Task<PreviousGamePage> loadPrevious(MatchDetail thisGame, Game game, Region region, Summoner summonerPls, List<RiotSharp.GameEndpoint.Player> fellowPlayers)
		{
			var staticApi = App.staticApi;
			var api = App.api;
			grouped = new ObservableCollection<GroupedPrevious>();
			var blueTeam = new GroupedPrevious { LongName = "Blue Team", ShortName = "B" };
			var redTeam = new GroupedPrevious { LongName = "Red Team", ShortName = "R" };
			var playersInPrev = new List<playersInPrevious>();

			var allItems = await staticApi.GetItemsAsync(region, ItemData.image);
			var allSummonerSpells = await staticApi.GetSummonerSpellsAsync(region, SummonerSpellData.image);

			playersInPrev.Add(new playersInPrevious { ChampionId = game.ChampionId, SummonerId = (int)summonerPls.Id });
			for (int i = 0; i < fellowPlayers.Count; i++)
			{
				playersInPrev.Add(new playersInPrevious { ChampionId = fellowPlayers[i].ChampionId, SummonerId = (int)fellowPlayers[i].SummonerId });
			}

			for (int i = 0; i < thisGame.Participants.Count; i++)
			{
				for (int j = 0; j < playersInPrev.Count; j++)
				{
					if (thisGame.Participants[i].ChampionId == playersInPrev[j].ChampionId)
					{
						var summoner = await api.GetSummonerAsync(region, playersInPrev[j].SummonerId);
						var champPlayed = await staticApi.GetChampionAsync(region, thisGame.Participants[i].ChampionId, ChampionData.image);
						var summonerSpellList = new List<string>
						{
							allSummonerSpells.SummonerSpells.FirstOrDefault(y => y.Value.Id == thisGame.Participants[i].Spell1Id).Value.Image.Full,
							allSummonerSpells.SummonerSpells.FirstOrDefault(y => y.Value.Id == thisGame.Participants[i].Spell2Id).Value.Image.Full
						};
						var itemList = new List<ItemStatic>
						{
							allItems.Items.FirstOrDefault((x) => x.Value.Id == thisGame.Participants[i].Stats.Item0).Value,
							allItems.Items.FirstOrDefault((x) => x.Value.Id == thisGame.Participants[i].Stats.Item1).Value,
							allItems.Items.FirstOrDefault((x) => x.Value.Id == thisGame.Participants[i].Stats.Item2).Value,
							allItems.Items.FirstOrDefault((x) => x.Value.Id == thisGame.Participants[i].Stats.Item3).Value,
							allItems.Items.FirstOrDefault((x) => x.Value.Id == thisGame.Participants[i].Stats.Item4).Value,
							allItems.Items.FirstOrDefault((x) => x.Value.Id == thisGame.Participants[i].Stats.Item5).Value,
							allItems.Items.FirstOrDefault((x) => x.Value.Id == thisGame.Participants[i].Stats.Item6).Value
						};
						var itemNameList = new List<string>();
						for (int k = 0; k < itemList.Count; k++)
						{
							if (itemList[k] != null)
								itemNameList.Add(itemList[k].Image.Full);
							else
								itemNameList.Add("Empty.png");
						}
						var prev = new PreviousExpanded(
							summoner.Name,
							(int)thisGame.Participants[i].Stats.ChampLevel,
							thisGame.Participants[i].Stats.Winner,
							FormatNumber((int)thisGame.Participants[i].Stats.GoldEarned),
							thisGame.Participants[i].Stats.MinionsKilled.ToString(),
							itemNameList,
							summonerSpellList,
							champPlayed.Image.Full,
							thisGame.Participants[i].Stats.Kills + "/" + thisGame.Participants[i].Stats.Deaths + "/" + thisGame.Participants[i].Stats.Assists
						);
						if (thisGame.Participants[i].TeamId == 100)
						{
							blueTeam.Add(prev);
						}
						else if (thisGame.Participants[i].TeamId == 200)
						{
							redTeam.Add(prev);
						}
					}
				}
			}
			grouped.Add(blueTeam);
			grouped.Add(redTeam);
			return new PreviousGamePage(grouped);
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
	public class GroupedPrevious : ObservableCollection<PreviousExpanded>
	{
		public string LongName { get; set; }
		public string ShortName { get; set; }
	}
	public class playersInPrevious
	{
		public int SummonerId { get; set; }
		public int ChampionId { get; set; }
	}
}