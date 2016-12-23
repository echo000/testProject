using System;
using LeagueApplication1;
using LeagueApplication1.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Google.MobileAds;
using UIKit;
using CoreGraphics;

[assembly: ExportRenderer(typeof(AdMobView), typeof(AdMobRenderer))]
namespace LeagueApplication1.iOS
{
	public class AdMobRenderer : ViewRenderer
	{
		const string AdmobID = "ca-app-pub-0525454145379686/8858650627";

		BannerView adView;
		bool viewOnScreen;

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement == null)
				return;

			if (e.OldElement == null)
			{
				adView = new BannerView(size: AdSizeCons.Banner, origin: new CGPoint(-10, 0))
				{
					AdUnitID = AdmobID,
					RootViewController = UIApplication.SharedApplication.Windows[0].RootViewController
				};

				adView.AdReceived += (sender, args) =>
				{
					Console.WriteLine("********** Banner Ad received");
					if (!viewOnScreen) this.AddSubview(adView);
					viewOnScreen = true;
				};

				adView.ReceiveAdFailed += (sender, args) =>
				{
					Console.WriteLine("********** BANNER AD FAILED");
				};

				Request request = Request.GetDefaultRequest();
#if DEBUG
				request.TestDevices = new string[] { "1a9b931fe9e9f25622e38c27e1aed953" };
#endif
				adView.LoadRequest(request);
				base.SetNativeControl(adView);
			}
		}
	}
}