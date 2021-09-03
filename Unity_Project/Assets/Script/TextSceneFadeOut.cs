using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class TextSceneFadeOut : MonoBehaviour
{
    [SerializeField]
    private Image clear_Image;

    [SerializeField]
    private GameObject clear_Base;


    private float time;

    [SerializeField]
    private float fadeTime;

    private bool timer = false;


    [SerializeField]
    private string nextScene;

    public void SceneFadeOut()
    {
            StartCoroutine(ClearBaseCoroutine());
            timer = true;
    }


    IEnumerator ClearBaseCoroutine()
    {
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
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextScene);
    }

}