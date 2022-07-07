using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    public Tutorial tutorial;
    public Text textField;

    private string text;

    public GameObject satellite;
    public Sprite disabledSprite;
    public Sprite enabledSprite;

    // small animation variables
    private float currentPos;
    private float maxPos = 0.05f;
    private float loopTime = 0.1f;

    private Vector3 startPos;

    private WaitForSecondsRealtime pause = new WaitForSecondsRealtime(3f);
    private WaitForSecondsRealtime delay = new WaitForSecondsRealtime(0.05f);

    private void Awake()
    {
        startPos = transform.localPosition;
    }

    private void OnEnable()
    {
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {

        for (int i = 0; i < 2; i++)
        {
            currentPos = 0;

            while (currentPos < maxPos)
            {
                currentPos += (1f / loopTime) * Time.unscaledDeltaTime * maxPos;
                transform.position += Vector3.up * (1f / loopTime) * Time.unscaledDeltaTime * maxPos;
                yield return null;
            }

            transform.localPosition = startPos + Vector3.up * maxPos;

            while (currentPos > 0)
            {
                currentPos -= (1f / loopTime) * Time.unscaledDeltaTime * maxPos;
                transform.position -= Vector3.up * (1f / loopTime) * Time.unscaledDeltaTime * maxPos;
                yield return null;
            }

            transform.localPosition = startPos;
            yield return delay;
        }

        yield return pause;

        StartCoroutine(Animation());
    }

    private void OnMouseDown()
    {
        text = tutorial.text[Tutorial.GetIndex()];
        textField.text = text;
        tutorial.UpdateSeenTutorials();
        satellite.GetComponent<SpriteRenderer>().sprite = disabledSprite;

        StartCoroutine(SetTextFieldActive());
    }

    private void OnMouseEnter()
    {
        satellite.GetComponent<SpriteRenderer>().sprite = enabledSprite;
    }

    private void OnMouseExit()
    {
        satellite.GetComponent<SpriteRenderer>().sprite = disabledSprite;
    }

    IEnumerator SetTextFieldActive()
    {
        yield return null;
        textField.transform.parent.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}