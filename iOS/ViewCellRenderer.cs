using Xamarin.Forms;
using LeagueApplication1.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using LeagueApplication1;

[assembly: ExportRenderer(typeof(ExpandedGameCell), typeof(StandardViewCellRenderer))]
namespace LeagueApplication1.iOS
{
	public class StandardViewCellRenderer : ViewCellRenderer
	{
		public override UIKit.UITableViewCell GetCell(Cell item, UIKit.UITableViewCell reusableCell, UIKit.UITableView tv)
		{
			var cell = base.GetCell(item, reusableCell, tv);
			cell.SelectionStyle = UITableViewCellSelectionStyle.None;
			return cell;
		}
	}
}