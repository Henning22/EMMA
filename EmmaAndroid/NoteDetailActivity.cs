using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using EmmaAndroid.Data;
using Android.Speech.Tts;

namespace EmmaAndroid
{
    [Activity(Label = "Notizen")]
    public class NoteDetailActivity : Activity, TextToSpeech.IOnInitListener
    {
        public TextToSpeech SpeechText;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SpeechText = new TextToSpeech(this, this);

            SetContentView(Resource.Layout.NoteDetail);
            var editTitle = FindViewById<EditText>(Resource.Id.editTitle);
            editTitle.Text = Intent.GetStringExtra("title") ?? "title";

            var editText = FindViewById<EditText>(Resource.Id.editText);
            editText.Text = Intent.GetStringExtra("text") ?? "";

            var id = Intent.GetStringExtra("id") ?? "";
            
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
            buttonSave.Click += (object sender, EventArgs e) =>
            {
                
                // NEW SHIT
                if(id == "")
                {
                    DBController.AddToDB(new NoteData(editTitle.Text, editText.Text));
                }
                else
                {
                    NoteData data = DBController.GetNote(int.Parse(id));
                    data.Title = editTitle.Text;
                    data.Text = editText.Text;

                    DBController.Update(data);
                }

                StartActivity(typeof(NoteActivity));

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