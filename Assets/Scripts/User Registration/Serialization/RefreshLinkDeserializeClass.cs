using Contaquest.Server;
using Newtonsoft.Json;

namespace User_Registration.Serialization
{
    public class RefreshLinkDeserializeClass
    {
        [JsonProperty("data")]
        public RefreshLinkData Data { get; set; }
    }
    
    public class RefreshLinkData
    {
        [JsonProperty("refreshEnjinLink")]
        public UserAccountData RefreshLink { get; set; }
    }
}