using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace LeagueApplication1
{
	public class FilterControl : View
	{
		public event EventHandler SelectedIndexChanged;

		public static readonly BindableProperty ItemsProperty =
			BindableProperty.Create(nameof(Items), typeof(List<string>), typeof(FilterControl), new List<string>());
		public List<string> Items
		{
			get
			{
				return GetValue(ItemsProperty) as List<string>;
			}
			set
			{
				SetValue(ItemsProperty, value);
			}
		}
		public static readonly BindableProperty SelectedIndexProperty =
			BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(FilterControl), 0, BindingMode.TwoWay);

		public int SelectedIndex
		{
			get
			{
				return (int)GetValue(SelectedIndexProperty);
			}
			set
			{
				SetValue(SelectedIndexProperty, value);
			}
		}
		public void OnSelectedIndexChanged(int newValue)
		{
			var args = new IndexChangedEventArgs()
			{
				NewValue = newValue,
				OldValue = SelectedIndex
			};

			SelectedIndex = newValue;

			if (SelectedIndexChanged != null)
			{
				SelectedIndexChanged(this, args);
			}
		}

		public static readonly BindableProperty TintColorProperty =
			BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(FilterControl), Color.Default, BindingMode.TwoWay);
		public Color TintColor
		{
			get
			{
				return (Color)GetValue(TintColorProperty);
			}
			set
			{
				SetValue(TintColorProperty, value);
			}
		}

	}
}