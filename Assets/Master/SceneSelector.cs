using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
	public void ChangeScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public static void GoBack()
    {
		SceneManager.LoadScene("ConstructorScene");
	}
}
