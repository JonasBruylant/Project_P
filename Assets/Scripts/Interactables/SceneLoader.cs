using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour, IInteractable
{
    public string SceneName;
    public float FadeTime = 2;
    public bool IsForLoadingIn = false;


    private GameObject _fadePlane;
    private Image _image;

    private void Start()
    {
        _fadePlane = GameObject.Find("FadePlane");
        _image = _fadePlane.GetComponent<Image>();
        if(IsForLoadingIn) StartCoroutine(FadeIntoScene());
    }


    public void Interact()
    {
       StartCoroutine(FadeToBlack());
    }

    private IEnumerator FadeIntoScene()
    {
        var startTime = Time.time;
        while (Time.time < startTime + FadeTime)
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, Mathf.Lerp(1f, 0f, (Time.time - startTime) / FadeTime));
            // TrapRotateParts[index].part.transform.rotation = Quaternion.Slerp(beginRotation, endRotation, (Time.time - startTime) / TrapRotateParts[index].rotationTime);
            yield return null;
        }
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);

    }

    private IEnumerator FadeToBlack()
    {
        var startTime = Time.time;

        while (Time.time < startTime + FadeTime)
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, Mathf.Lerp(0f, 1f, (Time.time - startTime) / FadeTime));
           // TrapRotateParts[index].part.transform.rotation = Quaternion.Slerp(beginRotation, endRotation, (Time.time - startTime) / TrapRotateParts[index].rotationTime);
            yield return null;
        }
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1f);
        SceneManager.LoadScene(SceneName);
    }
}
