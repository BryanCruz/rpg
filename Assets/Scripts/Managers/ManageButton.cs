using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageButton : MonoBehaviour
{
	public void NovoJogo()
	{
		SceneManager.LoadScene( "Lab5_RPGSetup" );
	}

	public void Creditos()
	{
		SceneManager.LoadScene( "Creditos" );
	}

	public void Sair()
	{
		Application.Quit();
	}
}
