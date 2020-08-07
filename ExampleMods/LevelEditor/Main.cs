/*	
 * SETUP STEPS
 *		Add References to the project to your games UnityEngine files and Assembly-CSharp file
 *		Then Modify the mod file as required
 * 
 * This test mod was made for The Messenger and allows you to always jump in mid air
 */
using System;
using UnityEngine;

namespace Mod
{
	public class Main : MonoBehaviour
	{
		public void Start()
		{
			GameObject gameObject = new GameObject();
			gameObject.AddComponent<LevelEditor.modComponent>();
			DontDestroyOnLoad(gameObject);
		}
	}
}