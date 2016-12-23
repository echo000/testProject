using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using RiotSharp;

namespace LeagueApplication1
{
	public class ChampionListView : ListView
	{
		ObservableCollection<GroupedChampions> champions;
		public ChampionListView()
		{
			var api = App.api;
			
			champions = new Champs();

			var championFreeToPlay = api.GetChampions(Region.euw, true);

			for (int i = 0; i < championFreeToPlay.Count; i++)
			{
				for (int j = 0; j < champions.Count; j++)
				{
					var freeToPlayChampion = champions[j].FirstOrDefault(x => x.champID == championFreeToPlay[i].Id);
					if (freeToPlayChampion != null)
					{
						freeToPlayChampion.Name.Insert(0, "<color:#80BFFF>");
						freeToPlayChampion.Name += " Free To Play";
					}
				}
			}

			var imageTemplate = new DataTemplate(typeof(ImageCell));
			imageTemplate.SetBinding(TextCell.TextProperty, "Name");
			imageTemplate.SetBinding(ImageCell.ImageSourceProperty, "Icon");
			imageTemplate.SetBinding(TextCell.DetailProperty, "Title");

			IsGroupingEnabled = true;
			GroupShortNameBinding = new Binding("ShortName");
			GroupDisplayBinding = new Binding("LongName");

			ItemTemplate = imageTemplate;
			ItemsSource = champions;
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.FillAndExpand;
		}
	}
}

