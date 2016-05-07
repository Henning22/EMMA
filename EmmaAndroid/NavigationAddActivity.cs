using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using EmmaAndroid.Data;
using Android.Speech.Tts;

namespace EmmaAndroid
{
    [Activity(Label = "Navigation")]
    public class NavigationAddActivity : Activity, TextToSpeech.IOnInitListener
    {
        public TextToSpeech SpeechText;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NavigationAdd);
            SpeechText = new TextToSpeech(this, this);

            var buttonHome = FindViewById<ImageButton>(Resource.Id.imageButton7);
            buttonHome.Click += (object sender2, EventArgs e2) =>
            {
                StartActivity(typeof(MainActivity));
            };

            var testSpeechButton = FindViewById<ImageButton>(Resource.Id.imageButton10);
            testSpeechButton.SetImageResource(Resource.Drawable.audio_out_deactivated);
            testSpeechButton.Click += delegate
            {
                Controller.SwitchTTSStatus();
                if (Controller.GetTTSStatus())
                {
                    testSpeechButton.SetImageResource(Resource.Drawable.audio_out_deactivated);
                    SpeechText.Speak("Automatische Sprachausgabe aktiviert!", QueueMode.Flush, null);
                }
                else
                {
                    testSpeechButton.SetImageResource(Resource.Drawable.audio_out);
                }
            };

            var buttonSave = FindViewById<Button>(Resource.Id.buttonSave);
            var editTitle = FindViewById<EditText>(Resource.Id.editTitle);
            var editAddress = FindViewById<EditText>(Resource.Id.editAddress); 

            buttonSave.Click += (object sender, EventArgs e) =>
            {
                Intent intent = new Intent(this, typeof(NavigationListActivity));
                if (Intent.GetStringExtra("type") == "hospital")
                { 
                    AppData.NavigationData.AddAddress(new AddressData(editTitle.Text, editAddress.Text, AddressType.hospital));
                    intent.PutExtra("type", "hospital");
                }
                else
                    AppData.NavigationData.AddAddress(new AddressData(editTitle.Text, editAddress.Text, AddressType.standard));

                StartActivity(intent);
            };

            var buttonFlashlight = FindViewById<ImageButton>(Resource.Id.imageButtonFlashlight);
            buttonFlashlight.Click += (object sender, EventArgs e) =>
            {
                Controller.CameraSwitch();
            };
        }

        public void OnInit(OperationResult status)
        {

        }
    }
}