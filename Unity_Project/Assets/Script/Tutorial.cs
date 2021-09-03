using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorial_Base;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private GameManager gameManager;


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            tutorial_Base.SetActive(true);
            gameManager.istutorial = true;
        }
    }

    public void Close()
    {
        tutorial_Base.SetActive(false);
        gameManager.istutorial = false;
    }

}
