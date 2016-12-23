using System;
using Xamarin.Forms;

namespace LeagueApplication1
{
	public class TwitchStreamCell : ViewCell
	{
		public TwitchStreamCell()
		{
			Height = 205;
			var nameLabel = new Label
			{
				FontSize = 20,
				TextColor = Color.White,
				BackgroundColor = Color.FromRgba(0, 0, 0, 0.6),
				HeightRequest = 36
			};
			nameLabel.SetBinding(Label.TextProperty, "Name");

			var descriptionLabel = new Label
			{
				FontSize = 14,
				TextColor = Color.White
			};
			descriptionLabel.SetBinding(Label.TextProperty, "Status");

			var viewLabel = new Label
			{
				FontSize = 14,
				TextColor = Color.White
			};
			viewLabel.SetBinding(Label.TextProperty, "Views");

			var myImage = new Image
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				//Aspect = Aspect.AspectFill
			};
			myImage.SetBinding(Image.SourceProperty, "Url");

			RelativeLayout layout = new RelativeLayout();

			layout.Children.Add(myImage,
				Constraint.Constant(2.5),
				Constraint.Constant(2.5),
				Constraint.RelativeToParent((parent) => { return parent.Width-5; }),
				Constraint.RelativeToParent((parent) => { return parent.Height-5; }));

			layout.Children.Add(nameLabel,
				Constraint.Constant(11),
				Constraint.Constant(167.5),
				Constraint.RelativeToParent((parent) => { return parent.Width-22; }),
				Constraint.RelativeToParent((parent) => { return 35; }));

			View = layout;
		}
	}
}
