using System;
using Xamarin.Forms;

namespace LeagueApplication1
{
	public class AbilityCell : ViewCell
	{
		public AbilityCell()
		{
			var champAbilityImage = new Image
			{
				WidthRequest = 50,
				HeightRequest = 50,
				Aspect = Aspect.AspectFill,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
			};
			champAbilityImage.SetBinding(Image.SourceProperty, "Image");

			var nameLabel = new Label()
			{
				TextColor = Color.Black
			};
			nameLabel.SetBinding(Label.TextProperty, "Name");

			var distanceLabel = new HtmlLabel
			{
				TextColor = Color.Black,
				FontSize = 15
			};
			distanceLabel.SetBinding(Label.TextProperty, "Description");


			var abilityDetailsLayout = new StackLayout
			{
				Padding = new Thickness(10, 0, 0, 0),
				Spacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { nameLabel, distanceLabel }
			};

			var champLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = {
					champAbilityImage, abilityDetailsLayout
				}
			};

			var cellLayout = new StackLayout
			{
				Padding = new Thickness(10, 5, 10, 5),
				//Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { champLayout }
			};

			this.View = cellLayout;
		}
	}
}

