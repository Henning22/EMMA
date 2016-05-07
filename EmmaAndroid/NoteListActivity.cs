using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using EmmaAndroid.Data;
using Android.Speech.Tts;

namespace EmmaAndroid
{
    [Activity(Label = "Notizen")]
    public class NoteListActivity : Activity, TextToSpeech.IOnInitListener
    {
        public TextToSpeech SpeechText;

        public void OnInit( OperationResult status)
        {
            
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SpeechText = new TextToSpeech(this, this);
            SetContentView(Resource.Layout.ListLayout);

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

            var lv = (LinearLayout)FindViewById(Resource.Id.linearLayout6);

            foreach (NoteData nd in DBController.GetAllNotes())
            { 
                var tw = new TextView(this);
                tw.SetTextSize(Android.Util.ComplexUnitType.Dip, 30);
                tw.Clickable = true;
                tw.Text = nd.Title;
                tw.SetTextColor(Android.Graphics.Color.Black);

                tw.Click += (object sender, EventArgs o) =>
                {
                    var activity = new Intent(this, typeof(NoteDetailActivity));

                    activity.PutExtra("title", nd.Title);
                    activity.PutExtra("text", nd.Text);
                    activity.PutExtra("id", nd.ID.ToString());
                    StartActivity(activity);
                };

                lv.AddView(tw);

                var div = new View(this);
                div.SetMinimumWidth(100);
                div.SetBackgroundColor(Android.Graphics.Color.DarkGray);
                div.SetMinimumHeight(2);            
                lv.AddView(div);
               
            }

            var buttonFlashlight = FindViewById<ImageButton>(Resource.Id.imageButtonFlashlight);
            buttonFlashlight.Click += (object sender, EventArgs e) =>
            {
                Controller.CameraSwitch();
            };

        }
    }
}