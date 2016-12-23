using TwitchLib.Controllers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TwitchLib.Models;

namespace LeagueApplication1
{
	public class TwitchApi
	{
		private TwitchController requester;
		private static TwitchApi instance;

		public static TwitchApi GetInstance(string clientId)
		{
			if (instance == null || TwitchRequesters.TwitchApi == null || clientId != TwitchRequesters.TwitchApi.ClientId)
			{
				instance = new TwitchApi(clientId);
			}
			return instance;
		}

		public TwitchApi(string clientId)
		{
			TwitchRequesters.TwitchApi = new TwitchController(clientId);
			requester = TwitchRequesters.TwitchApi;
		}

		public async Task<StreamsTop> GetStream(string requestUrl, TwitchController.RequestType type)
		{
			var json = await requester.CreateGetRequestAsync(requestUrl, type);
			var obj = (await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<StreamsTop>(json)));
			return obj;
		}
	}
}
