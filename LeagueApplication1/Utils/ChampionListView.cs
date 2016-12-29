using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using RiotSharp;
using System.Threading.Tasks;
using RiotSharp.StaticDataEndpoint;

namespace LeagueApplication1
{
	public class ChampionListView : ListView
	{
		ObservableCollection<Champ> champions;
		public ChampionListView()
		{
			var api = App.api;


			
			//champions = new Champs();

			var imageTemplate = new DataTemplate(typeof(ImageCell));
			imageTemplate.SetBinding(TextCell.TextProperty, "Name");
			imageTemplate.SetBinding(ImageCell.ImageSourceProperty, "Icon");
			imageTemplate.SetBinding(TextCell.DetailProperty, "Title");

			ItemTemplate = imageTemplate;
			ItemsSource = champions;
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.FillAndExpand;
		}
	}
}

