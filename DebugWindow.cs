using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Experimental.UIElements;

namespace UnityModLoader.Window
{
	public class Debug : MonoBehaviour
	{
		public Rect debugWindowRect = new Rect(20,20,400,600);
		public bool debugWindow = false;
		public Vector2 debugScrollBar = new Vector2 ();
		List<String> Logs = new List<String>();

        void Start() {
			Application.logMessageReceived += HandleLog;
		}

		void OnGUI() {
			GUI.skin = UnityModLoader.Window.UI.UISkin;
			if (debugWindow) {
				Cursor.visible = true;
				//Time.timeScale = 0.2f;
				debugWindowRect = GUI.Window (2, debugWindowRect, DebugWindow, "Debug");
            }
            else
            {
				Cursor.visible = false;
				Time.timeScale = 1f;
			}
		}

		public void DebugWindow(int windowID) {
			debugScrollBar = GUILayout.BeginScrollView (debugScrollBar,false,true);

			foreach (String s in Logs) {
				GUILayout.Label (s);
			}

			GUILayout.EndScrollView ();
            
			GUI.DragWindow ();
		}

		public void Update() {
			if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.P) || Input.GetKey (KeyCode.RightControl) && Input.GetKeyDown (KeyCode.P)) {
				debugWindow = !debugWindow;
				GetComponent<MainWindow>().consoleWindow = false;
				GetComponent<Explorer>().explorerWindow = false;
				GetComponent<Online>().onlineWindow = false;
			}

		}

		public void HandleLog (String logString, String stackTrace, LogType logType) {
			Logs.Add (logString);
			debugScrollBar.y = float.MaxValue;
		}
	}
}