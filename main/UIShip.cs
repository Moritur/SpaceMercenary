using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class UIShip : MonoBehaviour {

    [Tooltip("Unique name of this ship model (list can be edited in UIStaticData script)")]
    public UIStaticData.shipNames shipName;
    [HideInInspector]
    public UIStaticData.moduleList[] moduleList;    //list of modules player selected
    string path;                //path to save file
    int currentModule=0;        //module slot player is currently editing

    private void Awake()
    {
        UpdatePath();
        Load();
    }

    public void UpdatePath()
    {
        path = Application.persistentDataPath + "/" + "shipsave" + shipName + ".json";
    }

    public void SetCurrentModule(int id)
    {
        currentModule = id;
    }

    public void SetModule(UIStaticData.moduleList module)
    {
        moduleList[currentModule] = module;
    }

    public void Save()
    {
        using (StreamWriter streamWriter = new StreamWriter(path))
        {
            streamWriter.Write(JsonUtility.ToJson(this));
        }
    }

    public void Load()
    {
        if (!File.Exists(path)) { return; } //don't try loading file that doesn't exist
        using (StreamReader streamReader = new StreamReader(path))
        {
            JsonUtility.FromJsonOverwrite(streamReader.ReadToEnd(), this);
        }
    }
}