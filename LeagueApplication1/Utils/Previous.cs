using RiotSharp.GameEndpoint;
using System.Collections.Generic;

namespace LeagueApplication1
{
	public class Previous
	{
		public string Name { get; set; }
		public int Level { get; set; }
		public string Title { get; set; }
		public string Icon { get; set; }
		public bool Result { get; set; }
		public string DateTime { get; set; }
		public string GoldEarned { get; set; }
		public string CreepScore { get; set; }
		public string SummonerSpell1Icon { get; set; }
		public string SummonerSpell2Icon { get; set; }
		public string Item0Icon { get; set; }
		public string Item1Icon { get; set; }
		public string Item2Icon { get; set; }
		public string Item3Icon { get; set; }
		public string Item4Icon { get; set; }
		public string Item5Icon { get; set; }
		public string Item6Icon { get; set; }
		public Game ThisGame { get; set; }

		public bool Victory
		{
			get
			{
				return Result == true;
			}
		}
		public bool Loss
		{
			get
			{
				return Result == false;
			}
		}

		public Previous(string champPlayed, int level, bool result, string gold, string creepScore, List<string> itemNameList,
		List<string> summonerSpellList, string imagePath = "", string dateTime = "", string gameMode = "", Game thisGame = null)
		{
			ThisGame = thisGame;
			Name = champPlayed;
			Title = gameMode;
			Level = level;
			GoldEarned = gold;
			CreepScore = creepScore;
			Icon = imagePath;
			Result = result;
			DateTime = dateTime;
			SummonerSpell1Icon = summonerSpellList[0];
			SummonerSpell2Icon = summonerSpellList[1];
			Item0Icon = itemNameList[0];
			Item1Icon = itemNameList[1];
			Item2Icon = itemNameList[2];
			Item3Icon = itemNameList[3];
			Item4Icon = itemNameList[4];
			Item5Icon = itemNameList[5];
			Item6Icon = itemNameList[6];
		}
	}
}

