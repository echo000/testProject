using System.Linq;
using LeagueApplication1.iOS;
using Xamarin.Auth;
using Xamarin.Forms;

[assembly: Dependency(typeof(CredentialsService))]
namespace LeagueApplication1.iOS
{
	public class CredentialsService : ICredentialsService
	{
		public string UserName
		{
			get
			{
				var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
				return (account != null) ? account.Username : null;
			}
		}

		public string ProfileId
		{
			get
			{
				var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
				return (account != null) ? account.Properties["id"] : null;
			}
		}

		public string Region
		{
			get
			{
				var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
				return (account != null) ? account.Properties["Region"] : null;
			}
		}

		public void SaveCredentials(string userName, string region, string id)
		{
			if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(region))
			{
				Account account = new Account
				{
					Username = userName
				};
				account.Properties.Add("Region", region);
				account.Properties.Add("id", id);
				AccountStore.Create().Save(account, App.AppName);
			}

		}

		public void DeleteCredentials()
		{
			var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
			if (account != null)
			{
				AccountStore.Create().Delete(account, App.AppName);
			}
		}

		public bool DoCredentialsExist()
		{
			return AccountStore.Create().FindAccountsForService(App.AppName).Any() ? true : false;
		}
	}
}