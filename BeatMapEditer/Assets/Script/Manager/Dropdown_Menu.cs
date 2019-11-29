using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMode
{
    public static int Read = 0;
    public static int Save = 1;
    public static int Output = 2;
};

public class NotesMode
{
    public static int None = 0;
    public static int Put = 1;
    public static int Delete = 2;
};

public class Dropdown_Menu : MonoBehaviour
{


    [SerializeField] private Dropdown FileDropdown = null;
    [SerializeField] private Dropdown NotesDropdown = null;
    [SerializeField] private readWriteJsonFile jsonFile = null;
    [SerializeField] private EditManager manager = null;

    public void MenuPick()
    {
        if (FileDropdown.value == MenuMode.Read)
        {
            //jsonFile.readNotesFileDate();
        }

        if (FileDropdown.value == MenuMode.Save)
        {
            //jsonFile.writeNotesFileDate();
        }

        if (FileDropdown.value == MenuMode.Output)
        {
            //jsonFile.readNotesFileDate();
        }
    }

    public void NotesMenuPick()
    {
        if (NotesDropdown.value == NotesMode.None)
        {
        }

        if (NotesDropdown.value == NotesMode.Put)
        {
            //jsonFile.writeNotesFileDate();
        }

        if (NotesDropdown.value == NotesMode.Delete)
        {
            //jsonFile.readNotesFileDate();
        }
    }
}
