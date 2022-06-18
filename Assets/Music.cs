using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    private List<AudioSource> tracks = new List<AudioSource>();
    private int[] switchPoints = new int[7];

    private float minimalTrackLength = 0f; // tracks don't change for x seconds after switching

    private int currentTrack;

    private Ecosystem ecosystem;
    private float lastGas;

    private float maxVolume = 0.8f;

    private bool fading;

    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            tracks.Add(transform.GetChild(i).GetComponent<AudioSource>());
        }

        for (int i = 0; i < 7; i++)
        {
            switchPoints[i] = 166 * i;
        }

        ecosystem = GameObject.FindGameObjectWithTag("Planet_B").GetComponent<Ecosystem>();

        tracks[0].volume = maxVolume;
    }

    void Update()
    {
        float currentGas = ecosystem.GetCurrentGas();
        if (currentGas == lastGas || fading) return;

        int currentGasTrack = currentTrack;

        for (int i = 0; i < 6; i++)
        {
            int lowEnd = switchPoints[i];
            int highEnd = switchPoints[i + 1];

            if (currentGas > lowEnd && currentGas < highEnd) currentGasTrack = i;
            currentGasTrack = Mathf.Clamp(currentGasTrack, 0, 5);
        }

        if (currentGasTrack != currentTrack )
        {
            StartCoroutine(ChangeTrackTo(currentGasTrack));
        }

        lastGas = currentGas;
    }

    private IEnumerator ChangeTrackTo(int trackNumber)
    {
        fading = true;
        float currentVolume = 0f;

        while (currentVolume <= maxVolume)
        {
            tracks[trackNumber].volume += 0.1f * Time.unscaledDeltaTime;
            tracks[currentTrack].volume -= 0.1f * Time.unscaledDeltaTime;

            currentVolume += 0.1f * Time.unscaledDeltaTime;
            yield return null;
        }

        tracks[trackNumber].volume = maxVolume;
        tracks[currentTrack].volume = 0f;

        yield return new WaitForSecondsRealtime(minimalTrackLength);

        currentTrack = trackNumber;
        fading = false;
        yield break;
    }

}
