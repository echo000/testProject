using System;
using Xamarin.Forms;
using System.Collections.Generic;
using RiotSharp;
using System.Linq;

namespace LeagueApplication1
{
	public class TabbedPages : TabbedPage
	{
		public TabbedPages()
		{
			BackgroundColor = Color.Gray;
			Children.Add(new MasterPage() { Title = "Search", Icon = "Search.png" });
			Children.Add(new ChampsPage() { Title = "Champions", Icon = "Human.png" });
		}
	}

	public class ChampsPage : NavigationPage
	{
		public ChampsPage() : base(new ChampListPage())
		{
			BarBackgroundColor = Color.FromRgb(36, 36, 36);
			BarTextColor = Color.Green;
		}
	}
}