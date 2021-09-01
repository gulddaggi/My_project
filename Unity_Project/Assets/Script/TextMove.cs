using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextMove : MonoBehaviour
{

    [SerializeField]
    private Image[] images;

    [SerializeField]
    private Button button;

    [SerializeField]
    private string nextStage;


    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        StartCoroutine(TextMoveCoroutine());
    }

    IEnumerator TextMoveCoroutine()
    {

        for (int i = 0; i < images.Length; ++i)
        {
            if (i == 1)
            {
                images[0].transform.position = new Vector3(transform.position.x + 600f, transform.position.y + 150f, transform.position.z);
            }
            else if (i >= 2)
            {
                if (i % 2 == 0)
                {
                    images[i - 1].transform.position = new Vector3(transform.position.x - 600f, transform.position.y + 150f, transform.position.z);
                }
                else
                {
                    images[i - 1].transform.position = new Vector3(transform.position.x + 600f, transform.position.y + 150f, transform.position.z);
                }
                images[i - 2].transform.localScale = Vector3.zero;
            }

            images[i].gameObject.SetActive(true);

            yield return new WaitForSeconds(2.5f);
        }
        button.gameObject.SetActive(true);
    }

    public void Button()
    {
        SceneManager.LoadScene(nextStage);
    }
}
