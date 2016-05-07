using System;
using System.Collections.Generic;

using EmmaAndroid.Data;
using SQLite;
using System.IO;

namespace EmmaAndroid
{
    public static class DBController
    {

        public static void StartDB()
        {
            try
            {
                AppData.DB = GetCon();
                AppData.DB.CreateTable<AddressData>();
                AppData.DB.CreateTable<NoteData>();
            }
            catch (Exception e)
            {

            }
        }

        public static SQLiteConnection GetCon()
        {
            return new SQLiteConnection(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database.db"));
        }

        public static void AddToDB(AddressData data)
        {
            AppData.DB.Insert(data);
        }

        public static void AddToDB(NoteData data)
        {
            AppData.DB.Insert(data);
        }

        public static List<AddressData> GetAllAddresses()
        {
            var query = AppData.DB.Table<AddressData>();
            List<AddressData> list = new List<AddressData>();
            foreach (var stock in query)
                list.Add(stock);
            
            return list;
        }
        
        public static NoteData GetNote(int id)
        {
            foreach (NoteData d in AppData.DB.Table<NoteData>().Where(v => v.ID == id))
            {
                return d;
            }
            return null;
        }

        public static List<NoteData> GetAllNotes()
        {
            var list = new List<NoteData>();
            foreach (NoteData d in AppData.DB.Table<NoteData>())
            {
                list.Add(d);
            }
            return list;
        }

        internal static void Update(NoteData data)
        {
            AppData.DB.Update(data);
        }
    }
}