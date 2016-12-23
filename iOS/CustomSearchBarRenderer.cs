using System;
using CoreGraphics;
using LeagueApplication1;
using LeagueApplication1.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]
public class CustomSearchBarRenderer : SearchBarRenderer
{
	protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
	{
		base.OnElementChanged(e);

		var toolbar = new UIToolbar(new CGRect(0.0f,0.0f,Control.Frame.Size.Width,44.0f));

		toolbar.Items = new[]
		{
			new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
			new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { Control.ResignFirstResponder(); })
		};

		this.Control.InputAccessoryView = toolbar;
	}
}

