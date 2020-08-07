using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LevelEditor
{
    public static class SaveManager
    {
        public static SaveData saveData = new SaveData();

        public static bool writeSave()
        {

            string path = Application.dataPath + "/Managed/Mods/Testing/SunkenTemple.json";
            foreach(MapDataObject mdo in saveData.objectList)
            {
                Debug.Log("While saving we found: " + mdo.theName);
            }
            string data = JsonUtility.ToJson(saveData);
            Debug.Log(data);
            File.WriteAllText(path, data);

            return false;
        }
    }



    [Serializable]
    public struct SaveData
    {
        public List<MapDataObject> objectList;
    }
    [Serializable]
    public struct MapDataObject
    {
       // public GameObject theObject;
        public string theName;
    }

}
