using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LeagueApplication1.iOS
{
public class SegmentedView : View
	{
		public SegmentedView()
		{
			Items = new List<string>();
		}

		public IList<string> Items { get; private set; }
		public static readonly BindableProperty SelectedIndexProperty =
			BindableProperty.Create("SelectedIndex", typeof(int), typeof(SegmentedView), default(int),
				propertyChanged: (bindable, oldvalue, newValue) =>
				{
					var eh = ((SegmentedView)bindable).SelectedIndexChanged;
					if (eh != null)
						eh(bindable, EventArgs.Empty);
				});

		public int SelectedIndex
		{
			get { return (int)GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}

		public static readonly BindableProperty ColorProperty =
			BindableProperty.Create("Color", typeof(Color), typeof(SegmentedView), default(Color));

		public Color Color
		{
			get { return (Color)GetValue(ColorProperty); }
			set { SetValue(ColorProperty, value); }
		}

		public static readonly BindableProperty FontProperty =
			BindableProperty.Create("Font", typeof(Font), typeof(SegmentedView), default(Font));

		public Font Font
		{
			get { return (Font)GetValue(FontProperty); }
			set { SetValue(FontProperty, value); }
		}

		public event EventHandler SelectedIndexChanged;
	}
}

