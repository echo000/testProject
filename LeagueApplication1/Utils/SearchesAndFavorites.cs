using Xamarin.Forms;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using SQLite;

namespace LeagueApplication1
{
	public class SearchesAndFavorites
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public int summonerID { get; set; }
		public string Name { get; set; }
		public string Region { get; set; }
		public string Icon { get; set; }
	}

	public class GroupedSearchesAndFavorites : ObservableCollection<SearchesAndFavorites>
	{
		public string LongName { get; set; }
		public string ShortName { get; set; }
	}

	public class ItemList : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public ObservableCollection<SearchesAndFavorites> _items;

		public ObservableCollection<SearchesAndFavorites> Items
		{
			get { return _items; }
			set { _items = value; OnPropertyChanged("Items"); }
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged == null)
				return;
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public ItemList(List<SearchesAndFavorites> itemList)
		{
			Items = new ObservableCollection<SearchesAndFavorites>();
			foreach (SearchesAndFavorites itm in itemList)
			{
				Items.Add(itm);
			}
			MessagingCenter.Subscribe(this, "DeleteThis", (SearchesAndFavorites campaignController) =>
			{
				Items.Remove(campaignController);
				App.FavoriteDatabase.DeleteItem(campaignController.ID);
			});
		}
	}
}

