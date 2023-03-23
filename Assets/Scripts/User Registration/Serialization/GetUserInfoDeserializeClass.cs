using System;
using Contaquest.Server;
using Newtonsoft.Json;

namespace User_Registration.Serialization
{
    public class GetUserInfoDeserializeClass
    {
        [JsonProperty("data")]
        public GetAllInfoData GetAllInfoData { get; set; }
    }

    public class GetAllInfoData
    {
        [JsonProperty("getUserInfo")]
        public UserAccountData GetUserInfo { get; set; }
    }
}