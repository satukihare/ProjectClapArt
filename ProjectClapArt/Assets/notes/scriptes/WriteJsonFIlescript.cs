using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteJsonFIlescript : MonoBehaviour {

    //JSON書き込み用の実態
    [SerializeField] readWriteJsonFile rw_json_file = null;

    //書き込むファイル名
    [SerializeField] string write_file_name = "tutorialNotes.json";

    // Start is called before the first frame update
    void Start() {

        List<Bar> bars = new List<Bar>();

        {
            Bar bar = new Bar();
            bar.StartTime = 0;
            bar.Lingth = 4000;
            bar.Notes = new List<Note>();

            Note note0 = new Note(new Vector2(-1, 1), 500, 2500, Note.NOTE_TYPE.FLICK);
            Note note1 = new Note(new Vector2(1, 1), 1000, 3000, Note.NOTE_TYPE.FLICK);
            Note note2 = new Note(new Vector2(-1, -1), 1500, 3500, Note.NOTE_TYPE.FLICK);
            Note note3 = new Note(new Vector2(1, -1), 2000, 4000, Note.NOTE_TYPE.FLICK);
            bar.Notes.Add(note0);
            bar.Notes.Add(note1);
            bar.Notes.Add(note2);
            bar.Notes.Add(note3);
            bars.Add(bar);
        }
        {
            Bar bar = new Bar();
            bar.StartTime = 6000;
            bar.Lingth = 4000;
            bar.Notes = new List<Note>();

            Note note0 = new Note(new Vector2(-1, 1), 500, 2500, Note.NOTE_TYPE.FLICK);
            Note note1 = new Note(new Vector2(1, 1), 1500, 3500, Note.NOTE_TYPE.FLICK);
            Note note2 = new Note(new Vector2(1, -1), 2000, 4000, Note.NOTE_TYPE.FLICK);
            bar.Notes.Add(note0);
            bar.Notes.Add(note1);
            bar.Notes.Add(note2);
            bars.Add(bar);
        }
        {
            Bar bar = new Bar();
            bar.StartTime = 12000;
            bar.Lingth = 4000;
            bar.Notes = new List<Note>();

            Note note0 = new Note(new Vector2(-1, 1), 500, 2500, Note.NOTE_TYPE.FLICK);
            Note note1 = new Note(new Vector2(1, 1), 1750, 3750, Note.NOTE_TYPE.FLICK);
            Note note2 = new Note(new Vector2(1, -1), 2000, 4000, Note.NOTE_TYPE.FLICK);
            bar.Notes.Add(note0);
            bar.Notes.Add(note1);
            bar.Notes.Add(note2);
            bars.Add(bar);
        }

        if (rw_json_file != null) {
            //書き込み
            rw_json_file.writeNotesFileDate(this.write_file_name, bars , "アスロック米倉.asroc");
        }

    }

    // Update is called once per frame
    void Update() {

    }
}
