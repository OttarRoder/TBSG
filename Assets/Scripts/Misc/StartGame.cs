using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    Button startButton;

    void Start ()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(delegate { startGame(); });
	}
	
	void startGame()
    {
        SceneManager.LoadSceneAsync("Main");
    }
}
