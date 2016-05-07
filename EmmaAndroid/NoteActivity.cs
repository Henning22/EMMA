using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Speech.Tts;

namespace EmmaAndroid
{
    [Activity(Label = "Notizen")]
    public class NoteActivity : Activity, TextToSpeech.IOnInitListener
    {
        public TextToSpeech SpeechText;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.Note);
            SpeechText = new TextToSpeech(this, this);

            var buttonHome = FindViewById<ImageButton>(Resource.Id.imageButton7);
            buttonHome.Click += (object sender, EventArgs e2) =>
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

            var buttonNew = FindViewById<ImageButton>(Resource.Id.imageButtonNew);
            buttonNew.Click += (object sender, EventArgs e) =>
            {
                StartActivity(typeof(NoteDetailActivity));
            };

            var buttonList = FindViewById<ImageButton>(Resource.Id.imageButtonList);
            buttonList.Click += (object sender, EventArgs e) =>
            {
                var listActivity = new Intent(this, typeof(NoteListActivity));
                
                StartActivity(listActivity);
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