using System.Collections.Generic;

namespace EmmaAndroid.Data
{
    public class NavigationData
    {
        public AddressData HomeAddress;
        public List<AddressData> Addresses;
         

        public NavigationData()
        {
            
            Addresses = new List<AddressData>();
            DBController.StartDB();

            Load();
        }

        public void SaveAdress(AddressData data)
        {
            DBController.AddToDB(data);           
        }

        public void Load()
        {
            LoadHomeAddress();
            Addresses.AddRange(DBController.GetAllAddresses());
        }

        private void LoadHomeAddress()
        {

        }


        public void AddAddress(AddressData adr)
        {
            Addresses.Add(adr);
            SaveAdress(adr);
        }

        public void AddAddresses(List<AddressData> adr)
        {
            Addresses.AddRange(adr);
            foreach(AddressData data in adr)
            {
                SaveAdress(data);
            }
        }


        public void DeleteAddress(AddressData adr)
        {
            Addresses.Remove(adr);
        }

        internal List<string> GetAllAddresses()
        {
            var list = new List<string>();
            foreach (AddressData adrData in Addresses)
                list.Add(adrData.Address);

            return list;
        }

        internal List<AddressData> GetAddressesOfType(AddressType type)
        {
            List<AddressData> list = new List<AddressData>();

            //Return backwards
            if (type == AddressType.latest)
            {
                for(int i = Addresses.Count-1; i >= 0; i--)
                {
                    if (Addresses[i].Type == type)
                        list.Add(Addresses[i]);
                }
                return list;
            }

            foreach (AddressData data in Addresses)
            {
               if (data.Type == type)
                   list.Add(data);
            }
            
            return list;
        }
        //AND SO ON... 
    }
}