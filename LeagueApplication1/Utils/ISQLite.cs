using System;
using SQLite;
namespace LeagueApplication1
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection(string filename);
	}
}

