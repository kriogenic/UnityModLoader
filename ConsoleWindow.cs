using System;
using UnityEngine;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Security.Cryptography;

namespace UnityModLoader.Window
{
	public class Console : MonoBehaviour
	{
		public Rect consoleWindowRect = new Rect(20,20,375,600);
		public bool consoleWindow = true;
        public Vector2 modListScroll = new Vector2();

		public void Start() {
            ServicePointManager.ServerCertificateValidationCallback =
                   new RemoteCertificateValidationCallback(
                        delegate
                        { return true; }
                    );

            return;


		}

        public static string Hash(string stringToHash)
        {
            using (var sha1 = new SHA1Managed())
            {
                return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(stringToHash)));
            }
        }

        public void OnGUI() {
			GUI.skin = UnityModLoader.Window.UI.UISkin;
			if (consoleWindow) {
				consoleWindowRect = GUI.Window (0, consoleWindowRect, ConsoleWindow, "Console",GUI.skin.GetStyle("window"));
			}
		}

		public void ConsoleWindow(int windowID) {
			GUILayout.Label ("Kriogenic Mod Loader v0.8 Enabled");
			GUILayout.Label ("Press Ctrl + I to toggle this menu");
			GUILayout.Label ("Press Ctrl + O to toggle object explorer menu (dev)");
			GUILayout.Label ("Press Ctrl + P to toggle debug viewer (dev)");
			GUILayout.Label ("Press Ctrl + U to toggle online mod browser");
			GUILayout.Label ("Click on a mod to toggle it on and off (Requires Game Restart)");

            
            if (GUILayout.Button("Open mod folder"))
            {
                System.Diagnostics.Process.Start("explorer.exe", "/root,"+(Application.dataPath + "/Managed/Mods/").Replace('/','\\'));
            }
			if (PlayerPrefs.GetInt ("Disabled") == 0) {
				if (GUILayout.Button ("☑ | Enable / Disable Modloader (Requires Game Restart)")) {
					PlayerPrefs.SetInt ("Disabled", 1);
				}
			} else {
				if (GUILayout.Button ("☐ | Enable / Disable Modloader (Requires Game Restart)")) {
					PlayerPrefs.SetInt ("Disabled", 0);
				}
			}
			GUILayout.Label ("---");
			GUILayout.Label ("Loaded mods:");
            modListScroll = GUILayout.BeginScrollView(modListScroll);
			foreach (String path in Directory.GetFiles(Application.dataPath+"/Managed/Mods/","*.dll")) {
				if (PlayerPrefs.GetInt (Path.GetFileName (path)) == 0) {
					if (GUILayout.Button ("☑ | " + Path.GetFileName (path))) {
						PlayerPrefs.SetInt (Path.GetFileName (path), 1);
					}
				} else {
					if (GUILayout.Button ("☐ | " + Path.GetFileName (path))) {
						//Toggle
						PlayerPrefs.SetInt (Path.GetFileName (path), 0);
					}
				}
			}
            GUILayout.EndScrollView();

			GUI.DragWindow ();
		}

		public void Update() {
			if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.I) || Input.GetKey (KeyCode.RightControl) && Input.GetKeyDown (KeyCode.I)) {
				consoleWindow = !consoleWindow;
			}
		}
	}
}

