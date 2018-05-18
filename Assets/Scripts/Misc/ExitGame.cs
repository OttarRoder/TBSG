using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    Button exitButton;

	void Start ()
    {
        exitButton = GetComponent<Button>();
        exitButton.onClick.AddListener(delegate { exitGame(); });
	}
	
	void exitGame()
    {
        Application.Quit();
	}
}
