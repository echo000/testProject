using Xamarin.Forms;
using RiotSharp.StaticDataEndpoint;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Octane.Xam.VideoPlayer;
using Acr.UserDialogs;

namespace LeagueApplication1
{
	public class ChampionPage : ContentPage
	{
		public string skinString = "http://ddragon.leagueoflegends.com/cdn/img/champion/splash/{0}_{1}.jpg";
		public ObservableCollection<Skins> skins { get; set; }
		public FilterControl memeTest;

		public ChampionPage(ChampionStatic champion)
		{
			NavigationPage.SetHasNavigationBar(this, true);
			BackgroundColor = Color.FromRgb(36, 36, 36);

			memeTest = new FilterControl { Items = new List<string> { "Lore", "Tips", "Abilities" } };
			memeTest.SelectedIndex = 0;

			Title = champion.Name;

			skins = new ObservableCollection<Skins>();

			for (int i = 0; i < champion.Skins.Count; i++)
			{
				skins.Add(new Skins { ImageUrl = string.Format(skinString, champion.Key, champion.Skins[i].Num), Name = champion.Skins[i].Name });
				if (skins[i].Name == "default")
				{
					skins[i].Name = "Default";
				}
			}

			var images = new List<Image>();

			for (int i = 0; i < skins.Count; i++)
			{
				images.Add(new Image { Source = skins[i].ImageUrl });
			}

			SliderView slider = new SliderView(images[0],200, App.display.Width)
			{
				BackgroundColor = Color.Gray,
				TransitionLength = 200,
				StyleId = "SliderView",
				MinimumSwipeDistance = 50
			};

			for (int i = 1; i < images.Count; i++)
			{
				slider.Children.Add(images[i]);
			}

			var loreView = new ScrollView
			{
				Content = new HtmlLabel { Text = champion.Lore }
			};

			var tipsView = new ScrollView();

			var stack = new StackLayout();
			stack.Children.Add(new HtmlLabel { Text = "<u>Ally Tips</u>", FontSize = 20 });

			for (int i = 0; i < champion.AllyTips.Count; i++)
			{
				stack.Children.Add(new HtmlLabel { Text = champion.AllyTips[i] + "\n\n" });
			}

			stack.Children.Add(new HtmlLabel { Text = "<u>Enemy Tips</u>", FontSize = 20 });
			for (int i = 0; i < champion.EnemyTips.Count; i++)
			{
				stack.Children.Add(new HtmlLabel { Text = champion.EnemyTips[i] + "\n\n" });
			}

			tipsView.Content = stack;

			var abilityListView = new ListView
			{
				HasUnevenRows = true,
				BackgroundColor = Color.FromRgb(36, 36, 36)
			};

			var abilityTemplate = new DataTemplate(typeof(AbilityCell));

			var abilityList = new List<abilityView>();

			if (champion.Id != 420)
			{
				var stringId = champion.Id.ToString();
				if (champion.Id == 2 || champion.Id == 3 || champion.Id == 4 || champion.Id == 8)
				{
					abilityList.Add(new abilityView { Name = champion.Passive.Name, Description = champion.Passive.Description, Image = champion.Passive.Image.Full, Video = string.Format("https://lolstatic-a.akamaihd.net/champion-abilities/videos/mp4/000{0}_01.mp4", champion.Id) });

					for (int i = 0; i < champion.Spells.Count; i++)
					{
						abilityList.Add(new abilityView { Name = champion.Spells[i].Name, Description = champion.Spells[i].Description, Image = champion.Spells[i].Image.Full, Video = string.Format("https://lolstatic-a.akamaihd.net/champion-abilities/videos/mp4/000{0}_0{1}.mp4", champion.Id, i + 2) });
					}
				}
				if (champion.Id == 240)
				{
					abilityList.Add(new abilityView { Name = champion.Passive.Name, Description = champion.Passive.Description, Image = champion.Passive.Image.Full, Video = "No Video" });

					for (int i = 0; i < champion.Spells.Count; i++)
					{
						abilityList.Add(new abilityView { Name = champion.Spells[i].Name, Description = champion.Spells[i].Description, Image = champion.Spells[i].Image.Full, Video = "No Video" });
					}
				}
				if (champion.Id == 12 || champion.Id == 36 || champion.Id == 9 || champion.Id == 120 || champion.Id == 126 ||
						 champion.Id == 85 || champion.Id == 82 || champion.Id == 25 || champion.Id == 78 || champion.Id == 33 || 
				         champion.Id == 102 || champion.Id == 27 || champion.Id == 134 || champion.Id == 91 || champion.Id == 77 || 
				         champion.Id == 45 || champion.Id == 106 || champion.Id == 62)
				{
					abilityList.Add(new abilityView { Name = champion.Passive.Name, Description = champion.Passive.Description, Image = champion.Passive.Image.Full, Video = "No Video" });

					for (int i = 0; i < champion.Spells.Count; i++)
					{
						switch (stringId.Length)
						{
							case 1:
								abilityList.Add(new abilityView { Name = champion.Spells[i].Name, Description = champion.Spells[i].Description, Image = champion.Spells[i].Image.Full, Video = string.Format("http://cdn.leagueoflegends.com/champion-abilities/videos/mp4/000{0}_0{1}.mp4", champion.Id, i + 2) });
								break;
							case 2:
								abilityList.Add(new abilityView { Name = champion.Spells[i].Name, Description = champion.Spells[i].Description, Image = champion.Spells[i].Image.Full, Video = string.Format("http://cdn.leagueoflegends.com/champion-abilities/videos/mp4/00{0}_0{1}.mp4", champion.Id, i + 2) });
								break;
							case 3:
								abilityList.Add(new abilityView { Name = champion.Spells[i].Name, Description = champion.Spells[i].Description, Image = champion.Spells[i].Image.Full, Video = string.Format("http://cdn.leagueoflegends.com/champion-abilities/videos/mp4/0{0}_0{1}.mp4", champion.Id, i + 2) });
								break;
							case 4:
								abilityList.Add(new abilityView { Name = champion.Spells[i].Name, Description = champion.Spells[i].Description, Image = champion.Spells[i].Image.Full, Video = string.Format("http://cdn.leagueoflegends.com/champion-abilities/videos/mp4/{0}_0{1}.mp4", champion.Id, i + 2) });
								break;
						}
					}
				}
				else
				{
					switch (stringId.Length)
					{
						case 1:
							abilityList.Add(new abilityView { Name = champion.Passive.Name, Description = champion.Passive.Description, Image = champion.Passive.Image.Full, Video = string.Format("http://cdn.leagueoflegends.com/champion-abilities/videos/mp4/000{0}_01.mp4", champion.Id) });
							break;
						case 2:
							abilityList.Add(new abilityView { Name = champion.Passive.Name, Description = champion.Passive.Description, Image = champion.Passive.Image.Full, Video = string.Format("http://cdn.leagueoflegends.com/champion-abilities/videos/mp4/00{0}_01.mp4", champion.Id) });
							break;
						case 3:
							abilityList.Add(new abilityView { Name = champion.Passive.Name, Description = champion.Passive.Description, Image = champion.Passive.Image.Full, Video = string.Format("http://cdn.leagueoflegends.com/champion-abilities/videos/mp4/0{0}_01.mp4", champion.Id) });
							break;
						case 4:
							abilityList.Add(new abilityView { Name = champion.Passive.Name, Description = champion.Passive.Description, Image = champion.Passive.Image.Full, Video = string.Format("http://cdn.leagueoflegends.com/champion-abilities/videos/mp4/{0}_01.mp4", champion.Id) });
							break;
					}

					for (int i = 0; i < champion.Spells.Count; i++)
					{
						switch (stringId.Length)
						{
							case 1:
								abilityList.Add(new abilityView { Name = champion.Spells[i].Name, Description = champion.Spells[i].Description, Image = champion.Spells[i].Image.Full, Video = string.Format("http://cdn.leagueoflegends.com/champion-abilities/videos/mp4/000{0}_0{1}.mp4", champion.Id, i + 2) });
								break;
							case 2:
								abilityList.Add(new abilityView { Name = champion.Spells[i].Name, Description = champion.Spells[i].Description, Image = champion.Spells[i].Image.Full, Video = string.Format("http://cdn.leagueoflegends.com/champion-abilities/videos/mp4/00{0}_0{1}.mp4", champion.Id, i + 2) });
								break;
							case 3:
								abilityList.Add(new abilityView { Name = champion.Spells[i].Name, Description = champion.Spells[i].Description, Image = champion.Spells[i].Image.Full, Video = string.Format("http://cdn.leagueoflegends.com/champion-abilities/videos/mp4/0{0}_0{1}.mp4", champion.Id, i + 2) });
								break;
							case 4:
								abilityList.Add(new abilityView { Name = champion.Spells[i].Name, Description = champion.Spells[i].Description, Image = champion.Spells[i].Image.Full, Video = string.Format("http://cdn.leagueoflegends.com/champion-abilities/videos/mp4/{0}_0{1}.mp4", champion.Id, i + 2) });
								break;
						}
					}
				}
			}
			else
			{
				abilityList.Add(new abilityView { Name = champion.Passive.Name, Description = champion.Passive.Description, Image = champion.Passive.Image.Full, Video = "http://news.cdn.leagueoflegends.com/public/images/pages/illaoi-reveal/videos/Illaoi_P.mp4" });
				abilityList.Add(new abilityView { Name = champion.Spells[0].Name, Description = champion.Spells[0].Description, Image = champion.Spells[0].Image.Full, Video = "http://news.cdn.leagueoflegends.com/public/images/pages/illaoi-reveal/videos/Illaoi_Q.mp4" });
				abilityList.Add(new abilityView { Name = champion.Spells[1].Name, Description = champion.Spells[1].Description, Image = champion.Spells[1].Image.Full, Video = "http://news.cdn.leagueoflegends.com/public/images/pages/illaoi-reveal/videos/Illaoi_W.mp4" });
				abilityList.Add(new abilityView { Name = champion.Spells[2].Name, Description = champion.Spells[2].Description, Image = champion.Spells[2].Image.Full, Video = "http://news.cdn.leagueoflegends.com/public/images/pages/illaoi-reveal/videos/Illaoi_E.mp4" });
				abilityList.Add(new abilityView { Name = champion.Spells[3].Name, Description = champion.Spells[3].Description, Image = champion.Spells[3].Image.Full, Video = "http://news.cdn.leagueoflegends.com/public/images/pages/illaoi-reveal/videos/Illaoi_R.mp4" });
			}
			abilityListView.ItemTemplate = abilityTemplate;
			abilityListView.ItemsSource = abilityList;

			abilityListView.ItemTapped += (sender, e) =>
			{
				abilityListView.IsEnabled = false;
				var myListView = (ListView)sender;
				var myItem = (abilityView)myListView.SelectedItem;
				if (myItem.Video == "No Video")
				{
					UserDialogs.Instance.AlertAsync("Sadly, the ability " + myItem.Name + " doesn't have a video associated with it.", "Unable To Load", "Okay");
					abilityListView.IsEnabled = true;
					return;
				}

				var webview = new WebView
				{
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Source = new UrlWebViewSource
					{
						Url = myItem.Video
					}
				};

				var contentPage = new ContentPage
				{
					Content = webview,
					Title = myItem.Name,
					BackgroundColor = Color.FromRgb(36, 36, 36)
				};
				abilityListView.IsEnabled = true;
				Navigation.PushAsync(contentPage);
			};

			var filterViewChange = new ContentView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			filterViewChange.Content = loreView;
			memeTest.SelectedIndexChanged += (sender, e) =>
			{
				switch (memeTest.SelectedIndex)
				{
					case 0:
						filterViewChange.Content = loreView;
						break;
					case 1:
						filterViewChange.Content = tipsView;
						break;
					case 2:
						filterViewChange.Content = abilityListView;
						break;
				}
			};

			var stackLayout = new StackLayout();

			filterViewChange.HeightRequest = 440;

			stackLayout.Children.Add(slider);
			stackLayout.Children.Add(memeTest);
			stackLayout.Children.Add(filterViewChange);
			var adView = new AdMobView { WidthRequest = 320, HeightRequest = 50 };
			stackLayout.Children.Add(adView);

			Content = stackLayout;
		}
	}
	public class Skins
	{
		public string ImageUrl { get; set; }
		public string Name { get; set; }
	}

	public class abilityView
	{
		public string Image { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Video { get; set; }
	}

	/*public class skinsLayout : ContentView
	{
		public skinsLayout()
		{
			var label = new Label();
			label.SetBinding(Label.TextProperty, "Name");
			var image = new Image();
			image.SetBinding(Image.SourceProperty, "ImageUrl");

			var stack = new StackLayout
			{
				Children =
				{
					label,
					image
				}
			};
			Content = stack;
		}
	}*/
}