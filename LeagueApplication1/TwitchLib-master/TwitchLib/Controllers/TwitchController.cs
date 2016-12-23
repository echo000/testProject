using System.Net.Http;
using TwitchLib.Models;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace TwitchLib.Controllers
{
    public class TwitchController
    {
        public enum RequestType
        {
            Kraken,
            Tmi
        }
		private readonly HttpClient httpClient;
        public string ClientId { get; set; }
		public string queryStringParam;

        public TwitchController(string clientId)
        {
            Twitch = new Twitch(this);
			ClientId = clientId;
			queryStringParam = string.Format("&client_id={0}&api_version=3",ClientId);
			httpClient = new HttpClient();
        }

        public Twitch Twitch { get; set; }

		public async Task<string> CreateGetRequestAsync(string requestUrl, RequestType type)
		{
			var prefix = "";

			switch (type)
			{
				case RequestType.Kraken:
					prefix = "https://api.twitch.tv/kraken/";
					break;
				case RequestType.Tmi:
					prefix = "https://tmi.twitch.tv/";
					break;
			}

			var request = PrepareRequest(prefix, requestUrl, HttpMethod.Get);
			return await GetResultAsync(request);
		}

		protected HttpRequestMessage PrepareRequest(string prefix, string requestUrl, HttpMethod httpMethod)
		{
			var url = string.Format("{0}{1}{2}", prefix, requestUrl,queryStringParam);
			return new HttpRequestMessage(httpMethod, url);
		}

        protected async Task<string> GetResultAsync(HttpRequestMessage request)
        {
			var result = string.Empty;
			//httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-id", ClientId);
			//httpClient.DefaultRequestHeaders.Add("Client-id", ClientId);
			//httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.twitchtv.v3+json"));

			using (var response = httpClient.GetAsync(request.RequestUri).Result)
			{
				HttpResponseMessage responseObj = await httpClient.SendAsync(request);
                using (var content = response.Content)
				{
					result = await content.ReadAsStringAsync();
				}
				return result;
            }
        }
    }
}