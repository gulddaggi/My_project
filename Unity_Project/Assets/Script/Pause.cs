using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject pause_Go;

    private SaveNLoad saveNLoad;

    private GameManager gameManager;

    void Start()
    {
        saveNLoad = FindObjectOfType<SaveNLoad>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!gameManager.isPause)
            {
                OpenMenu();
            }
            else
            {
                CloseMenu();
            }
        }
    }

    private void OpenMenu()
    {
        gameManager.isPause = true;
        pause_Go.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        gameManager.isPause = false;
        pause_Go.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ClickReStart()
    {
        CloseMenu();
        saveNLoad.InGameLoad();
    }

    public void ClickExit()
    {
        Application.Quit();
    }

}
