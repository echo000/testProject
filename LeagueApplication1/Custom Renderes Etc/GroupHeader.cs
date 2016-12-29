using System;
using Xamarin.Forms;

namespace LeagueApplication1
{
	public class GroupHeader : ViewCell
	{
		public GroupHeader()
		{
			this.Height = 25;
			var title = new Label
			{
				Font = Font.SystemFontOfSize(NamedSize.Small, FontAttributes.Bold),
				TextColor = Color.Black,
				VerticalOptions = LayoutOptions.FillAndExpand
			};

			title.SetBinding(Label.TextProperty, "LongName");

			View = new StackLayout
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 25,
				Padding = 5,
				Orientation = StackOrientation.Horizontal,
				Children = { title }
			};
		}
	}
}
