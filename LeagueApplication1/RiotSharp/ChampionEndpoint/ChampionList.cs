using Newtonsoft.Json;
using System.Collections.Generic;
using LeagueApplication1;

namespace RiotSharp.ChampionEndpoint
{
    public class ChampionList
    {
		[Preserve]
		[JsonConstructor]
		internal ChampionList() { }
        /// <summary>
        /// List of Champions.
        /// </summary>
        [JsonProperty("champions")]
        public List<Champion> Champions { get; set; }
    }
}
