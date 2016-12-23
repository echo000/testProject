using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using Octane.Xam.VideoPlayer.iOS;
using Xamarin.InAppPurchase;
using Google.MobileAds;
using ImageCircle.Forms.Plugin.iOS;
using XLabs.Ioc;
using XLabs.Platform.Device;

namespace LeagueApplication1.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		internal static InAppPurchaseManager PurchaseManager;
		public static bool IsOnline { get; set; }

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			UITabBar.Appearance.BarTintColor = UIColor.FromRGB(32, 32, 32);
			UITabBar.Appearance.SelectedImageTintColor = UIColor.Green;
			global::Xamarin.Forms.Forms.Init();

			ImageCircleRenderer.Init();

			var resolverContainer = new SimpleContainer();

			resolverContainer.Register<IDevice>(t => AppleDevice.CurrentDevice);

			Resolver.SetResolver(resolverContainer.GetResolver());

			LoadApplication(new App());

			//var x = typeof(Xamarin.Forms.Themes.DarkThemeResources);
			//x = typeof(Xamarin.Forms.Themes.LightThemeResources);
			//x = typeof(Xamarin.Forms.Themes.iOS.UnderlineEffect);

			PurchaseManager = new InAppPurchaseManager();

			FormsVideoPlayer.Init("8DC996370723DC4CB87088B387234B2DCAE7189C");

			return base.FinishedLaunching(app, options);
		}
		public override void WillTerminate(UIApplication application)
		{
			base.WillTerminate(application);
		}
		/// <summary>
		/// Gets the window.
		/// </summary>
		/// <returns>UIWindow.</returns>
		[Export("window")]
		public UIWindow GetWindow()
		{
			return UIApplication.SharedApplication.Windows[0];
		}
	}
}

