using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuMode
{
    public static int Read = 1;
    public static int Save = 2;
};

public class NotesMode
{
    public static int Put = 0;
    public static int Delete = 1;
};

public class Dropdown_Menu : MonoBehaviour
{
    [SerializeField] private Dropdown TypeDropdown = null;
    [SerializeField] private Dropdown FileDropdown = null;
    [SerializeField] private Dropdown NotesDropdown = null;
    [SerializeField] private readWriteJsonFile jsonFile = null;
    [SerializeField] private EditManager manager = null;
    [SerializeField] private AudioSource source = null;
    public void MenuPick()
    {
        if (FileDropdown.value == MenuMode.Read)
        {
            manager.Bars = jsonFile.readNotesFileDate("test.json");
            Debug.Log("JSONを読み込みました");
            manager.DataRestart(false);
        }

        if (FileDropdown.value == MenuMode.Save)
        {
            jsonFile.writeNotesFileDate("Sub_Create.json", manager.Bars, source.clip.name);
            Debug.Log("JSONに書き込みました");
        }
        FocusOut();
    }

    public void NotesMenuPick()
    {
        if (NotesDropdown.value == NotesMode.Put)
        {
            manager.EditMode = true;
        }

        if (NotesDropdown.value == NotesMode.Delete)
        {
            manager.EditMode = false;
        }
        FocusOut();
    }

    public void PickType()
    {
      manager.Type = TypeDropdown.value;
        FocusOut();
    }

    public void FocusOut()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
