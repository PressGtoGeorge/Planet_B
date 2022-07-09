using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FlyIntoSpace : MonoBehaviour
{
    private float speed = 8f;

    private AudioSource source;

    IEnumerator Start()
    {
        yield return null;
        yield return null;

        source = gameObject.AddComponent<AudioSource>();
        source.clip = GameState.flyAudio;
        source.outputAudioMixerGroup = GameState.effectsGroup;
        source.playOnAwake = false;
    }

    public void Fly()
    {
        gameObject.SetActive(true);
        StartCoroutine(Flying());
    }

    private IEnumerator Flying()
    {
        source.pitch = Random.Range(0.75f, 1.25f);
        if (gameObject.GetComponent<RocketAnimation>() == null) source.Play();

        transform.parent = null;

        if (transform.GetChild(0).GetComponent<Character>() != null)
            transform.GetChild(0).GetComponent<Character>().SetMoving(false);

        if (gameObject.GetComponent<RocketAnimation>() != null)
            gameObject.GetComponent<RocketAnimation>().StopAnimation();

        float currentDistance = 0f;
        float goalDistance = 9001f;

        while (currentDistance < goalDistance)
        {
            currentDistance += speed * (Time.deltaTime / Time.timeScale);
            transform.position += transform.up * speed * (Time.deltaTime / Time.timeScale);
            yield return null;
        }

        Destroy(gameObject);
    }
}