using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace UnityModLoader
{
	public class ModLoader : MonoBehaviour
	{

        public void Start()
        {

            string path = Application.dataPath + "/Managed/Mods/";
            Directory.CreateDirectory(path);

            foreach (string d in Directory.GetDirectories(path))
            {
                Debug.Log("WE LOOKIN FOR IT");
                if (File.Exists(d + "/mod.dll"))
                {
                    Debug.Log("WE FOUND IT");
                    LoadMod(d + "/mod.dll");
                }
            }

            //foreach (String file in Directory.GetFiles(Application.dataPath + "/Managed/Mods/", "*.dll"))
           // {
           //     LoadMod(file);
           // }
        }

        public static void LoadMod(string path)
        {
            if (PlayerPrefs.GetInt(Path.GetFileName(path)) == 0)
            {
                Type type = Assembly.LoadFrom(path).GetType("Mod.Main");
                MethodInfo method = type.GetMethod("Start");
                method.Invoke(Activator.CreateInstance(type), null);
            }
        }
    }
}