using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOver_Base;

    private SaveNLoad saveNLoad;

    private GameManager gameManager;

    void Start()
    {
        saveNLoad = FindObjectOfType<SaveNLoad>();
        gameManager = FindObjectOfType<GameManager>();
    }


    public void GameOverOn()
    {
        gameOver_Base.SetActive(true);
        gameManager.isGameOver = true;
    }

    public void ClickReStart()
    {
        gameOver_Base.SetActive(false);
        gameManager.isGameOver = false;
        saveNLoad.InGameLoad();
        ItemPickUp.isHandgun = true;
        ItemPickUp.isStick = true;
    }

    public void ClickExit()
    {
        Application.Quit();
    }



}
