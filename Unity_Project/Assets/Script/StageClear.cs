using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class StageClear : MonoBehaviour
{
    [SerializeField]
    private Image clear_Image;

    [SerializeField]
    private GameObject clear_Base;

    [SerializeField]
    private Transform player;

    private float time;

    [SerializeField]
    private float fadeTime;

    private bool timer = false;

    private bool isPlaying = false;

    [SerializeField]
    private string textScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            StartCoroutine(ClearBaseCoroutine());
            timer = true;
            Debug.Log("ÀÎ½Ä");
        }
    }

    private void ImageFade()
    {
        if (isPlaying == true)
        {
            return;
        }

        StartCoroutine(ClearBaseCoroutine());
    }

    IEnumerator ClearBaseCoroutine()
    {
        isPlaying = true;
        clear_Base.SetActive(true);
        Color color = clear_Image.color;
        time = 0f;

        while (color.a >= 0f)
        {
            time += Time.deltaTime / fadeTime;

            color.a = Mathf.Lerp(0f, 1f, time);

            clear_Image.color = color;

            yield return null;

        }
        isPlaying = false;
    }

    private void Update()
    {
        if (clear_Image.color.a == 1f)
        {
            StartCoroutine(SceneLoadCoroutine());
        }
    }

    IEnumerator SceneLoadCoroutine()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(textScene);
    }

}
