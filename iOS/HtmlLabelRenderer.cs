using System.ComponentModel;
using Foundation;
using LeagueApplication1;
using LeagueApplication1.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]

namespace LeagueApplication1.iOS
{
	public class HtmlLabelRenderer : LabelRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);

			if (Control != null && Element != null && !string.IsNullOrWhiteSpace(Element.Text))
			{
				var attr = new NSAttributedStringDocumentAttributes();
				var nsError = new NSError();
				attr.DocumentType = NSDocumentType.HTML;

				UIKit.UIFont font = Control.Font;
				string fontName = font.Name;
				System.nfloat fontSize = font.PointSize;
				string htmlContents = "<span style=\"font-family: '" + fontName + "'; font-size: " + fontSize + "\">" + Element.Text + "</span>";
				var myHtmlData = NSData.FromString(htmlContents, NSStringEncoding.Unicode);
				Control.Lines = 0;
				Control.AttributedText = new NSAttributedString(myHtmlData, attr, ref nsError);
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == Label.TextProperty.PropertyName)
			{
				if (Control != null && Element != null && !string.IsNullOrWhiteSpace(Element.Text))
				{
					var attr = new NSAttributedStringDocumentAttributes();
					var nsError = new NSError();
					attr.DocumentType = NSDocumentType.HTML;

					UIKit.UIFont font = Control.Font;
					string fontName = font.Name;
					System.nfloat fontSize = font.PointSize;
					string htmlContents = "<span style=\"font-family: '" + fontName + "'; font-size: " + fontSize + "\">" + Element.Text + "</span>";
					var myHtmlData = NSData.FromString(htmlContents, NSStringEncoding.Unicode);
					Control.Lines = 0;
					Control.AttributedText = new NSAttributedString(myHtmlData, attr, ref nsError);
				}
			}
		}
	}
}