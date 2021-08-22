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
                Debug.Log(hitInfo.transform.name + "»πµÊ");
                Destroy(hitInfo.transform.gameObject);
                TextDisappear();
            }
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward + Vector3.up, out hitInfo, pickUpRange, layerMask))
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
}
