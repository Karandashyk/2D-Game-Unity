using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {


	public void StartGame() 
	{
		Application.LoadLevel (1);
	}

	public void LoadGame () {
		Application.LoadLevel (PlayerPrefs.GetInt("levelNew"));
	}

	public void ExitGame() 
	{
		Application.Quit ();
	}
}
