using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SwitchPlanet_2 : MonoBehaviour
{
    private GameObject planet_A;
    private GameObject planet_B;

    private bool moving;
    private float speed = 12f;

    private AudioSource switchSource;

    public Camera planet_B_Camera;
    public Camera planet_A_Camera;

    private CameraRatio planet_B_Ratio;
    private CameraRatio planet_A_Ratio;

    private Vector3 cameraOffset;

    private bool switched;

    private PostProcessVolume planet_A_post;
    private PostProcessVolume planet_B_post;

    public PostProcessVolume backgroundPost;

    private void Start()
    {
        planet_A = GameObject.FindGameObjectWithTag("Planet_A");
        planet_B = GameObject.FindGameObjectWithTag("Planet_B");

        planet_A_Ratio = planet_A_Camera.GetComponent<CameraRatio>();
        planet_B_Ratio = planet_B_Camera.GetComponent<CameraRatio>();

        switchSource = gameObject.GetComponent<AudioSource>();

        cameraOffset = planet_A.transform.position - planet_A_Camera.transform.position;
        cameraOffset = new Vector3(cameraOffset.x, cameraOffset.y, 0); //  + Vector3.right * (1.1f);

        planet_A_post = planet_A_Camera.transform.GetChild(0).GetComponent<PostProcessVolume>();
        planet_B_post = planet_B_Camera.transform.GetChild(0).GetComponent<PostProcessVolume>();
    }

    private void OnMouseDown()
    {
        StartSwitch();
    }

    public void StartSwitch()
    {
        if (moving == false) StartCoroutine(Move2());
    }

    private IEnumerator Move2()
    {
        moving = true;
        GameState.switching = true;

        switchSource.Play();

        float dir;
        if (switched)
        {
            dir = -1f;
            planet_A_Camera.depth = 1;
            planet_B_Camera.depth = 2;

        }
        else
        {
            dir = 1f;
            planet_A_Camera.depth = 2;
            planet_B_Camera.depth = 1;
        }

        float startSize = 5f;
        float endSize = 15f;

        float currentSize = startSize;

        while (currentSize <= endSize)
        {
            planet_B_Camera.orthographicSize += (Time.deltaTime / Time.timeScale) * speed * dir;
            planet_B_Ratio.orthographicStartSize += (Time.deltaTime / Time.timeScale) * speed * dir;

            planet_A_Camera.orthographicSize -= (Time.deltaTime / Time.timeScale) * speed * dir;
            planet_A_Ratio.orthographicStartSize -= (Time.deltaTime / Time.timeScale) * speed * dir;

            planet_B_Camera.transform.position -= cameraOffset * (Time.deltaTime / Time.timeScale) * speed / (endSize - startSize) * dir;
            planet_A_Camera.transform.position += (cameraOffset + Vector3.right * (1.1f)) * (Time.deltaTime / Time.timeScale) * speed / (endSize - startSize) * dir;

            if (switched == false)
            {
                currentSize = planet_B_Camera.orthographicSize;

                planet_B_post.weight = (currentSize - startSize) / (endSize - startSize);
                planet_A_post.weight = 1 - planet_B_post.weight;

            }
            else
            {
                currentSize = planet_A_Camera.orthographicSize;

                planet_A_post.weight = (currentSize - startSize) / (endSize - startSize);
                planet_B_post.weight = 1 - planet_A_post.weight;
            }

            /*
            if (currentSize <= 10)
            {
                backgroundPost.profile.GetSetting<LensDistortion>().intensity.value -= (Time.deltaTime / Time.timeScale) * speed * 20;
            }
            else
            {
                backgroundPost.profile.GetSetting<LensDistortion>().intensity.value += (Time.deltaTime / Time.timeScale) * speed * 20;
            }
            */
            yield return null;
        }

        // backgroundPost.profile.GetSetting<LensDistortion>().intensity.value = 0;

        if (switched == false)
        {
            planet_B_Camera.orthographicSize = endSize;
            planet_A_Camera.orthographicSize = startSize;

            planet_B_Ratio.orthographicStartSize = endSize;
            planet_A_Ratio.orthographicStartSize = startSize;

            planet_B_Camera.transform.position = (-1f) * cameraOffset + Vector3.forward * (-10f);
            planet_A_Camera.transform.position = planet_A.transform.position + Vector3.forward * (-10f) + Vector3.right * (1.1f);

            planet_B_post.weight = 1;
            planet_A_post.weight = 0;
        }
        else
        {
            planet_B_Camera.orthographicSize = startSize;
            planet_A_Camera.orthographicSize = endSize;

            planet_B_Ratio.orthographicStartSize = startSize;
            planet_A_Ratio.orthographicStartSize = endSize;

            planet_B_Camera.transform.position = Vector3.forward * (-10f);
            planet_A_Camera.transform.position = planet_A.transform.position + (-1f) * cameraOffset + Vector3.forward * (-10f);

            planet_B_post.weight = 0;
            planet_A_post.weight = 1;
        }

        moving = false;
        switched = !switched;
        GameState.switched = !GameState.switched;
        GameState.switching = false;
    }
}