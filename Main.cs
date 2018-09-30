using System;
using UnityEngine;

namespace UnityModLoader
{
	public class Main : MonoBehaviour
	{
		GameObject modObject;

		public void Start()
		{

			if (!GameObject.Find ("Kriogenic Unity Mod Loader")) {
				modObject = new GameObject ();
				if (PlayerPrefs.GetInt ("Disabled") == 0) {
					modObject.AddComponent<ModLoader> ();
				}
				modObject.AddComponent<UnityModLoader.Window.Debug> ();
				modObject.AddComponent<UnityModLoader.Window.Explorer> ();
				modObject.AddComponent<UnityModLoader.Window.Console> ();
				modObject.AddComponent<UnityModLoader.Window.Online> ();
				modObject.AddComponent<UnityModLoader.Window.UI> ();
				GameObject.DontDestroyOnLoad (modObject);
                modObject.name = "Kriogenic Unity Mod Loader";
            }
        }
	}
}