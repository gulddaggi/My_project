using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerate : MonoBehaviour
{
    [SerializeField]
    private EnemyController enemyCon;

    [SerializeField]
    private GameObject card;

    public bool isCardGenerate = false;

    private bool cardCheck = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (enemyCon.isEnt || enemyCon.isShocked)
        {
            GenerateCard();
        }
    }

    private void GenerateCard()
    {
        if (!cardCheck)
        {
            GameObject key = Instantiate(card, transform.position + new Vector3(0.5f, 0.3f, 0.5f), Quaternion.identity);
            key.name = "Card";
            isCardGenerate = true;
            cardCheck = true;
        }
            
    }
}
