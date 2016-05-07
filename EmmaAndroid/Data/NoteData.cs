using SQLite;

namespace EmmaAndroid.Data
{
    public class NoteData
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }

        public NoteData(string title, string text)
        {
            Text = text;
            Title = title;
        }

        public NoteData()
        {

        }
    }
}