using SQLite;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;

namespace LeagueApplication1
{
	public class RecentDatabase
	{
		static object locker = new object();
		SQLiteConnection database;
		public RecentDatabase()
		{
			database = DependencyService.Get<ISQLite>().GetConnection("RecentDatabase.db3");
			database.CreateTable<SearchesAndFavorites>();
		}
		public List<SearchesAndFavorites> GetItems()
		{
			return (from i in database.Table<SearchesAndFavorites>() select i).ToList();
		}
		public SearchesAndFavorites GetItem(int id)
		{
			return database.Table<SearchesAndFavorites>().FirstOrDefault(x => x.ID == id);
		}
		public SearchesAndFavorites GetItem(string name)
		{
			return database.Table<SearchesAndFavorites>().FirstOrDefault(x => x.Name == name);
		}
		public int DeleteItem(int id)
		{
			return database.Delete<SearchesAndFavorites>(id);
		}
		public void Clear()
		{
			database.DeleteAll<SearchesAndFavorites>();
		}
		public int SaveItem(SearchesAndFavorites item)
		{
			lock (locker)
			{
				if (item.ID != 0)
				{
					database.Update(item);
					return item.ID;
				}
				else {
					return database.Insert(item);
				}
			}
		}
	}
}

