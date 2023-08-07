using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCameraController : MonoBehaviour
{
    public Image fadeImage;
    public Transform centerTransform;
    public Vector2 zoneSize;
    public float fadeTime;
    public float panSpeed;
    public float showTime;
    public bool debug;

    private Vector2 direction;
    private Color fadeColor;
    private float origZ;
    private bool isFadeIn = false;
    private bool isFadeOut = false;
    private bool isShowing = false;

    void Start()
    {
        fadeImage.fillAmount = 1f;
        origZ = transform.transform.position.z;
    }

    void Update()
    {
        float newX = transform.position.x + direction.x * Time.deltaTime * panSpeed;
        float newY = transform.position.y + direction.y * Time.deltaTime * panSpeed;
        transform.position = new Vector3(newX, newY, origZ);
        if (!isShowing)
        {
            if (isFadeIn)
            {
                fadeColor.a = Mathf.Clamp01(fadeColor.a - Time.deltaTime / fadeTime); // Clamp01 to restrict the amount taken from alpha channel between 0 and 1.
                fadeImage.color = fadeColor;
            }
            else if (isFadeOut)
            {
                fadeColor.a = Mathf.Clamp01(fadeColor.a + Time.deltaTime / fadeTime); // Clamp01 to restrict the amount taken from alpha channel between 0 and 1.
                fadeImage.color = fadeColor;
            }
            else
            {
                InitMoveVectors();
                StartCoroutine(FadingInRoutine());
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (debug) Gizmos.DrawWireCube(centerTransform.position, zoneSize);
    }

    public void InitMoveVectors()
    {
        float newX = centerTransform.position.x + Random.value * zoneSize.x - zoneSize.x / 2f;
        float newY = centerTransform.position.y + Random.value * zoneSize.y - zoneSize.y / 2f;
        transform.position = new Vector3(newX, newY, origZ);
        direction = Random.onUnitSphere;
    }

    private IEnumerator FadingOutRoutine()
    {
        isFadeOut = true;
        yield return new WaitForSeconds(fadeTime);
        isFadeOut = false;
        InitMoveVectors();
        StartCoroutine(FadingInRoutine());
    }

    private IEnumerator FadingInRoutine()
    {
        isFadeIn = true;
        yield return new WaitForSeconds(fadeTime);
        isFadeIn = false;
        StartCoroutine(ShowingRoutine());

    }

    private IEnumerator ShowingRoutine()
    {
        isShowing = true;
        yield return new WaitForSeconds(showTime);
        isShowing = false;
        StartCoroutine(FadingOutRoutine());
    }
}
