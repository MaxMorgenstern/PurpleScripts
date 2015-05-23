using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PurpleGUI
{
	public class GUIManager : MonoBehaviour
	{
		private static GUIManager instance;

		private List<GameObject> canvasInstance;

		public GUIManager ()
		{
			
		}


		// CANVAS ////////////////////////////

		public GameObject CreateCanvas()
		{
			return CreateCanvas ("PurpleCanvas");
		}

		public GameObject CreateCanvas(string name)
		{
			GameObject tmpCanvasInstance = (GameObject)Instantiate (Resources.Load ("PurpleCanvas"), new Vector3 (0, 100, 100), Quaternion.identity);
			tmpCanvasInstance.name = name;
			canvasInstance.Add (tmpCanvasInstance);
			return tmpCanvasInstance;
		}

		public GameObject GetCanvas()
		{
			return canvasInstance.FirstOrDefault ();
		}

		public GameObject GetCanvas(string name)
		{
			return canvasInstance.Find (x => x.name == name );
		}


		// BUTTON ////////////////////////////

		public GameObject CreateButton(string buttonName, Func<string, int> function)
		{
			return CreateButton (buttonName, function, string.Empty, GetCanvas ());
		}

		public GameObject CreateButton(string buttonName, Func<string, int> function, string parameter)
		{
			return CreateButton (buttonName, function, parameter, GetCanvas ());
		}

		public GameObject CreateButton(string buttonName, Func<string, int> function, GameObject canvas)
		{
			return CreateButton (buttonName, function, string.Empty, canvas);
		}

		public GameObject CreateButton(string buttonName, Func<string, int> function, string parameter, GameObject canvas)
		{
			GameObject buttonInstance = (GameObject)Instantiate (Resources.Load ("PurpleButton"), new Vector3 (0, 100, 100), Quaternion.identity);
			buttonInstance.transform.SetParent (canvas.transform, false);
			buttonInstance.name = buttonName;

			Text txt = buttonInstance.GetComponentInChildren<Text>(); 
			txt.text = buttonName;

			Button buttonObject = buttonInstance.GetComponent<Button>();
			buttonObject.onClick.AddListener(delegate{function(parameter);});

			return buttonInstance;
		}

		public GameObject GetButton()
		{
			return null;
		}


	}
}
