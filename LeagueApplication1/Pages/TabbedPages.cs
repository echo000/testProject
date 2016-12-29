using System;
using Xamarin.Forms;
using System.Collections.Generic;
using RiotSharp.SummonerEndpoint;
using RiotSharp;
using System.Linq;

namespace LeagueApplication1
{
	public class TabbedPages : TabbedPage
	{
		public TabbedPages(Summoner summoner, Region region)
		{
			Children.Add(new StreamsPage() { Title = "Streams", Icon = "Streams.png" });
			Children.Add(new ChampsPage() { Title = "Champions", Icon = "Human.png" });
			Children.Add(new SearchNavPage() { Title = "Search", Icon = "Search.png" });
			Children.Add(new ProfPage(summoner, region) { Title = "Profile", Icon = "Profile.png" });
		}
	}

	public class ChampsPage : NavigationPage
	{
		public ChampsPage() : base(new ChampListPage())
		{
		}
	}

	public class StreamsPage : NavigationPage
	{
		public StreamsPage() : base(new StreamPage())
		{
		}
	}

	public class SearchNavPage : NavigationPage
	{
		public SearchNavPage() : base(new SearchTabPage())
		{
		}
	}

	public class ProfPage : NavigationPage
	{
		public ProfPage(Summoner summoner, Region region) : base(new ProfilePage(summoner, region))
		{
		}
	}
}