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
			Detail = new NavigationPage(new SearchPage(favMaster));

		}
	}
}
