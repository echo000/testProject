using System;
namespace LeagueApplication1
{
	public class Champ
	{
		public string Name { get; set; }
		public string Icon { get; set; }
		public string Title { get; set; }
		public int champID { get; set; }

		public Champ(string name, string title, int id)
		{
			this.Name = name;
			this.Title = title;
			this.champID = id;
		}
	}
}

