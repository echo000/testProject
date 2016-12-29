namespace LeagueApplication1
{
	public interface ICredentialsService
	{
		string UserName { get; }

		string ProfileId { get; }

		string Region { get; }

		void SaveCredentials(string userName, string region, string id);

		void DeleteCredentials();

		bool DoCredentialsExist();
	}
}