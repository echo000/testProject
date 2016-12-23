using System.Diagnostics;
using Xamarin.Forms;
using RiotSharp;
using XLabs.Ioc;
using XLabs.Platform.Device;

namespace LeagueApplication1
{
	public class App : Application
	{
		public static RiotApi api;
		public static StaticRiotApi staticApi;
		public static TwitchApi twitchApi;
		static FavoriteDatabase favoriteDatabase;
		static RecentDatabase recentDatabase;
		public static IDevice device;
		public static IDisplay display;

		public App()
		{
			api = RiotApi.GetInstance("RGAPI-16b454d1-ed2a-4661-966f-788fa899e702", 500, 30000);
			staticApi = StaticRiotApi.GetInstance("RGAPI-16b454d1-ed2a-4661-966f-788fa899e702");
			twitchApi = TwitchApi.GetInstance("4d0rotb2vg2vsmk57k7hz2kghy071go");
			Current.Resources = new ResourceDictionary();
			MainPage = new TabbedPages();
			device = Resolver.Resolve<IDevice>();
			display = device.Display;

			var navigationStyle = new Style(typeof(NavigationPage));
			var barColorSetter = new Setter { Property = NavigationPage.BarBackgroundColorProperty, Value = Color.FromRgb(36,36,36) };
			var barTextColorSetter = new Setter { Property = NavigationPage.BarTextColorProperty, Value = Color.Green };
			navigationStyle.Setters.Add(barColorSetter);
			navigationStyle.Setters.Add(barTextColorSetter);
			Current.Resources.Add(navigationStyle);
		}
		public static FavoriteDatabase FavoriteDatabase
		{
			get
			{
				if (favoriteDatabase == null)
				{
					favoriteDatabase = new FavoriteDatabase();
				}
				return favoriteDatabase;
			}
		}
		public static RecentDatabase RecentDatabase
		{
			get
			{
				if (recentDatabase == null)
				{
					recentDatabase = new RecentDatabase();
				}
				return recentDatabase;
			}
		}

		public int ResumeAtTodoId { get; set; }

		protected override void OnStart()
		{
			Debug.WriteLine("OnStart");
			if (Properties.ContainsKey("ResumeAtTodoId"))
			{
				var rati = Properties["ResumeAtTodoId"].ToString();
				Debug.WriteLine("rati=" + rati);
				if (!string.IsNullOrEmpty(rati))
				{
					Debug.WriteLine("rati = " + rati);
					ResumeAtTodoId = int.Parse(rati);

					if (ResumeAtTodoId >= 0)
					{
						//ar todoPage = new TodoItemPageX();
						//todoPage.BindingContext = Database.GetItem(ResumeAtTodoId);

						//MainPage.Navigation.PushAsync(
							//todoPage,
							//false); // no animation
					}
				}
			}
		}
		protected override void OnSleep()
		{
			Debug.WriteLine("OnSleep saving ResumeAtTodoId = " + ResumeAtTodoId);
			// the app should keep updating this value, to
			// keep the "state" in case of a sleep/resume
			Properties["ResumeAtTodoId"] = ResumeAtTodoId;
		}
		protected override void OnResume()
		{
			Debug.WriteLine("OnResume");
		}
	}
}

