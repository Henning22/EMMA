using Android.Hardware;
using SQLite;
using Java.IO;

namespace EmmaAndroid.Data
{
    public static class AppData
    {
        public static NavigationData NavigationData = new NavigationData();

        public static Camera camera = Camera.Open();
        public static SQLiteConnection DB = DBController.GetCon();
        public static SQLiteConnection MedicDB = DBController.GetCon();

        public static File _file;
        public static File _dir;
        public static Android.Graphics.Bitmap bitmap;

        public static bool TTSActivated = true;
    }
}