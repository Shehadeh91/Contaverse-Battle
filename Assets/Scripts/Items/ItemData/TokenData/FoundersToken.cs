using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Sirenix.OdinInspector;

namespace Contaquest.Metaverse.Data
{
    [System.Serializable]
    public class FoundersToken : TokenData
    {
        [JsonProperty(PropertyName = "Tier")]
        [ReadOnly] public string itemTierString;
        [NonSerialized] public FoundersTokenTier foundersTokenTier;

        public override void InitializeValuesBeforeSerialization()
        {
            base.InitializeValuesBeforeSerialization();

            itemTierString = foundersTokenTier.ToString();

            itemTierString = Regex.Replace(itemType, "([a-z](?=[A-Z]|[0-9])|[A-Z](?=[A-Z][a-z]|[0-9])|[0-9](?=[^0-9]))", "$1 ");
        }
    }
}
