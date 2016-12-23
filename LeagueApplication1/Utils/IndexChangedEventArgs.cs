using System;

namespace LeagueApplication1
{
	public class IndexChangedEventArgs : EventArgs
	{
		public int OldValue { get; set; }

		public int NewValue { get; set; }
	}
}