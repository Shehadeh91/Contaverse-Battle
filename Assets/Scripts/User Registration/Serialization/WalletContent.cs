namespace User_Registration.Serialization
{
    public class WalletContent
    {
        public string index;
        public Token token;
        public int value;
    }

    public class Token
    {
        public string id;
        public string name;
        public string itemURI;
        public long meltValue;
    }
}