using System;
using RiotSharp;
using Xamarin.Forms;

namespace LeagueApplication1
{
	public class CustomSearchBar : SearchBar
	{
		/*public const string ReturnKeyPropertyName = "ReturnKeyType";

		public CustomSearchBar() { }

		public static readonly BindableProperty ReturnKeyTypeProperty = BindableProperty.Create(
			propertyName: ReturnKeyPropertyName,
			returnType: typeof(ReturnKeyTypes),
			declaringType: typeof(CustomSearchBar),
			defaultValue: ReturnKeyTypes.Done
		);

		public ReturnKeyTypes ReturnKeyType
		{
			get { return (ReturnKeyTypes)GetValue(ReturnKeyTypeProperty); }
			set { SetValue(ReturnKeyTypeProperty, value); }
		}*/
	}
	public enum ReturnKeyTypes : int
	{
		Default,
		Go,
		Google,
		Join,
		Next,
		Route,
		Search,
		Send,
		Yahoo,
		Done,
		EmergenyCall,
		Continue
	}
}

