using SQLite;

namespace EmmaAndroid.Data
{
    public class AddressData
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }

        public AddressType Type { get; set; }

        public AddressData(string title, string adr, AddressType type)
        {
            Title = title;
            Address = adr;
            Type = type;
        }

        public AddressData()
        {
        }
    }
}