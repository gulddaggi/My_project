using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public string sceneName;

}

public class SaveNLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();

    private GameManager gameManager;

    [SerializeField]
    private ItemPickUp itemPickUp;


    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";

    void Awake()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Save";
        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        }
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Title")
        {
            Save();
        }
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Save()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            saveData.sceneName = SceneManager.GetActiveScene().name;


            string json = JsonUtility.ToJson(saveData);

            File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);
        }
        else
        {
            saveData.sceneName = "Stage1";

            string json = JsonUtility.ToJson(saveData);

            File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);
        }
        

        
    }

    public void Load()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJason = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJason);

            SceneManager.LoadScene(saveData.sceneName);

            gameManager.isPause = false;
            gameManager.canPlayerMove = true;
            ItemPickUp.isHandgun = true;
            ItemPickUp.isStick = true;

        }
        else
        {
        }
    }

    public void InGameLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


    }

}
