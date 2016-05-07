using Android.Content;
using Android.Hardware;
using EmmaAndroid.Data;

namespace EmmaAndroid
{
    public static class Controller
    {
        public static void StartDB()
        {
            
            DBController.StartDB();
        }
        public static Intent GetHomeNavigation()
        {

            var geoUri = Android.Net.Uri.Parse("geo:0,0?q= navigiere Malteser Hilfsdienst e.V. Stadtgliederung Freiburg");
            return new Intent(Intent.ActionView, geoUri);
        }

        internal static Intent GetNavigation(string textInput)
        {
            var geoUri = Android.Net.Uri.Parse("geo:0,0?q= navigiere " + textInput);
            return new Intent(Intent.ActionView, geoUri);
        }

        internal static void CameraSwitch()
        {
            var parameters = AppData.camera.GetParameters();

            if (parameters.FlashMode == Camera.Parameters.FlashModeOff)
                parameters.FlashMode = Camera.Parameters.FlashModeTorch;
            else
                parameters.FlashMode = Camera.Parameters.FlashModeOff;

            AppData.camera.SetParameters(parameters);
            AppData.camera.StopPreview();
        }

        public static bool GetTTSStatus()
        {
            return AppData.TTSActivated;
        }

        internal static void SwitchTTSStatus()
        {
            AppData.TTSActivated = !AppData.TTSActivated;
        }
    }
}