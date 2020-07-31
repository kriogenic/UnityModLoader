/*
 * To use this mod loader it is required that you find a suitable place to inject the following code using dnSpy
 *     Assembly dll = Assembly.LoadFile(Directory.GetCurrentDirectory() + "\\UnityModLoader.dll");
 *     Type t = dll.GetType("UnityModLoader.Main");
 *     object obj = dll.CreateInstance(t.Name);
 *     MethodInfo m = t.GetMethod("Start");
 *     m.Invoke(null, null); 
 */
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
namespace UnityModLoader
{
	public static class Main
	{
		static GameObject modObject;
		public static void Start()
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

		public static void Starts()
        {
			Console.WriteLine("HERE WE ARE");
        }
	}
}