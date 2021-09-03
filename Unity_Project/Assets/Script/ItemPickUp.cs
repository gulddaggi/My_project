using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickUp : MonoBehaviour
{
    //Ω¿µÊ ∞≈∏Æ
    [SerializeField]
    private float pickUpRange;

    //æ∆¿Ã≈€ »πµÊ ø©∫Œ
    private bool isPickUp = false;

    //√Êµπ¡§∫∏ ¿˙¿Â
    private RaycastHit hitInfo;

    //æ∆¿Ã≈€ø°∏∏ π›¿¿
    [SerializeField]
    private LayerMask layerMask;

    //æ∆¿Ã≈€ »πµÊæ»≥ª ≈ÿΩ∫∆Æ
    [SerializeField]
    private Text pickUPText;

    [SerializeField]
    private Status status;

    [SerializeField]
    private int firstAidKit;

    [SerializeField]
    private int shield;

    public bool isCard = false;

    public static bool isHandgun = false;

    public static bool isStick = false;

    void Update()
    {
        CheckItem();
        BeforePickUp();
    }

    //æ∆¿Ã≈€ »πµÊ ¿‘∑¬ √º≈©
    private void BeforePickUp()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            PickUp();
        }
    }

    //æ∆¿Ã≈€ »πµÊ
    private void PickUp()
    {
        if (isPickUp)
        {
            if (hitInfo.transform != null)
            {
                Destroy(hitInfo.transform.gameObject);
                TextDisappear();
                if (hitInfo.transform.name == "FirstAidKit")
                {
                    FirstAidKit();
                }
                else if (hitInfo.transform.name == "Shield")
                {
                    Shield();
                }
                else if (hitInfo.transform.name == "Card")
                {
                    Card();
                }
                else if (hitInfo.transform.name == "handgun")
                {
                    Handgun();
                }
                else if (hitInfo.transform.name == "stick")
                {
                    Stick();
                }
            }

        }
    }


    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, pickUpRange, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                TextAppear();
            }
        }
        else
        {
            TextDisappear();
        }
    }

    //»πµÊ æ»≥ªπÆ µÓ¿Â
    private void TextAppear()
    {
        isPickUp = true;
        pickUPText.gameObject.SetActive(true);
        pickUPText.text = hitInfo.transform.name + "»πµÊ" + "<color=yellow>" + "(E)" + "</color>";
    }

    //»πµÊ æ»≥ªπÆ ªÁ∂Û¡¸
    private void TextDisappear()
    {
        isPickUp = false;
        pickUPText.gameObject.SetActive(false);
    }

    private void FirstAidKit()
    {
        status.IncreaseHP(firstAidKit);
    }

    private void Shield()
    {
        status.IncreaseShield(shield);
    }

    private void Card()
    {
        isCard = true;
    }

    private void Handgun()
    {
        isHandgun = true;
    }

    private void Stick()
    {
        isStick = true;
    }
}
