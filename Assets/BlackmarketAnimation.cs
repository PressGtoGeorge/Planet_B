using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackmarketAnimation : MonoBehaviour
{
    private Vector3 originalCompSize;
    private Vector3 originalCharSpriteSize;
    private Vector3 holeMaxSize = new Vector3(2.8f, 1f, 0f);
    private Vector3 originalThoughtbubbleSize;

    private Vector3 originalCharSpritePos;

    public Transform characterSprite;
    public Transform holeSprite;
    public Transform thoughtBubble;

    private GameObject spriteMask_1;

    private Character charScript;

    private bool animating;

    public bool getsMount;

    void Start()
    {
        originalCompSize = transform.localScale;
        originalCharSpriteSize = transform.GetChild(0).localScale;
        originalThoughtbubbleSize = thoughtBubble.localScale;

        spriteMask_1 = transform.GetChild(3).gameObject;

        charScript = gameObject.GetComponent<Character>();
    }

    void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(EnterAnimation());
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            StartCoroutine(ExitAnimation());
        }*/
    }

    public void StartEnterAnimation()
    {
        if (animating == false) StartCoroutine(EnterAnimation());
    }

    public void StartExitAnimation()
    {
        if (animating == false) StartCoroutine(ExitAnimation());
    }

    private IEnumerator EnterAnimation()
    {
        if (animating) yield break;

        animating = true;
        charScript.SetMoving(false);

        originalCharSpritePos = characterSprite.localPosition;

        // make hole appear
        float speed = 1f;
        float progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * speed;
            holeSprite.localScale = holeMaxSize * progress;
            yield return null;
        }

        holeSprite.localScale = holeMaxSize;

        // make character fall into hole
        progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * speed;
            characterSprite.localPosition += Vector3.down * Time.deltaTime * speed * 2f;
            thoughtBubble.localScale -= originalThoughtbubbleSize * Time.deltaTime * speed;

            yield return null;
        }

        characterSprite.localPosition = originalCharSpritePos + Vector3.down * 2f;
        thoughtBubble.localScale = Vector3.zero;

        // hole disappears
        spriteMask_1.SetActive(true);
        progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * speed;
            holeSprite.localScale = holeMaxSize - holeMaxSize * progress;
            yield return null;
        }

        holeSprite.localScale = Vector3.zero;

        // reposition comp
        transform.Rotate(Vector3.forward, 180f);
        transform.localPosition = Vector3.up * (charScript.radiusPlanet_A - charScript.blackmarketPathDepth);

        characterSprite.GetComponent<SpriteRenderer>().flipX = !characterSprite.GetComponent<SpriteRenderer>().flipX;

        // reposition wheels so that they match the flipped image
        Vector3 localPos_0 = gameObject.GetComponent<Character>().characterRenderer[1].transform.localPosition;
        localPos_0 = new Vector3(localPos_0.x * -1f, localPos_0.y, localPos_0.z);
        gameObject.GetComponent<Character>().characterRenderer[1].transform.localPosition = localPos_0;
        gameObject.GetComponent<Character>().characterRenderer[1].gameObject.GetComponent<Rotate>().speed *= -1f;

        localPos_0 = gameObject.GetComponent<Character>().characterRenderer[2].transform.localPosition;
        localPos_0 = new Vector3(localPos_0.x * -1f, localPos_0.y, localPos_0.z);
        gameObject.GetComponent<Character>().characterRenderer[2].transform.localPosition = localPos_0;
        gameObject.GetComponent<Character>().characterRenderer[2].gameObject.GetComponent<Rotate>().speed *= -1f;

        // hole reappers
        progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * speed;
            holeSprite.localScale = holeMaxSize * progress;
            yield return null;
        }

        // character reappers
        progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * speed;
            characterSprite.localPosition += Vector3.up * Time.deltaTime * speed * 2f;

            yield return null;
        }

        characterSprite.localPosition = originalCharSpritePos;

        // hole disappears
        spriteMask_1.SetActive(false);
        progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * speed;
            holeSprite.localScale = holeMaxSize - holeMaxSize * progress;
            yield return null;
        }

        holeSprite.localScale = Vector3.zero;

        if (GameState.gameOver == false || charScript.onPlanet_A) charScript.SetMoving(true);
        animating = false;
        
        if (getsMount)
        {
            getsMount = false;
            charScript.animator.SetBool("mount", true);
        }

        yield break;
    }

    private IEnumerator ExitAnimation()
    {
        if (animating) yield break;

        animating = true;
        charScript.SetMoving(false);
        
        originalCharSpritePos = characterSprite.localPosition;

        // make hole appear
        float speed = 1f;
        float progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * speed;
            holeSprite.localScale = holeMaxSize * progress;
            yield return null;
        }

        holeSprite.localScale = holeMaxSize;

        // make character fall into hole
        progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * speed;
            characterSprite.localPosition += Vector3.down * Time.deltaTime * speed * 2f;

            yield return null;
        }

        characterSprite.localPosition = originalCharSpritePos + Vector3.down * 2f;

        // hole disappears
        spriteMask_1.SetActive(true);
        progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * speed;
            holeSprite.localScale = holeMaxSize - holeMaxSize * progress;
            yield return null;
        }

        holeSprite.localScale = Vector3.zero;

        // reposition comp
        transform.Rotate(Vector3.forward, 180f);
        transform.localPosition = Vector3.up * (charScript.radiusPlanet_A - charScript.surfacePathDepth);

        characterSprite.GetComponent<SpriteRenderer>().flipX = !characterSprite.GetComponent<SpriteRenderer>().flipX;

        // reposition wheels so that they match the flipped image
        Vector3 localPos_0 = gameObject.GetComponent<Character>().characterRenderer[1].transform.localPosition;
        localPos_0 = new Vector3(localPos_0.x * -1f, localPos_0.y, localPos_0.z);
        gameObject.GetComponent<Character>().characterRenderer[1].transform.localPosition = localPos_0;
        gameObject.GetComponent<Character>().characterRenderer[1].gameObject.GetComponent<Rotate>().speed *= -1f;

        localPos_0 = gameObject.GetComponent<Character>().characterRenderer[2].transform.localPosition;
        localPos_0 = new Vector3(localPos_0.x * -1f, localPos_0.y, localPos_0.z);
        gameObject.GetComponent<Character>().characterRenderer[2].transform.localPosition = localPos_0;
        gameObject.GetComponent<Character>().characterRenderer[2].gameObject.GetComponent<Rotate>().speed *= -1f;

        // hole reappers
        progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * speed;
            holeSprite.localScale = holeMaxSize * progress;
            yield return null;
        }

        // character reappers
        progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * speed;
            characterSprite.localPosition += Vector3.up * Time.deltaTime * speed * 2f;
            thoughtBubble.localScale += originalThoughtbubbleSize * Time.deltaTime * speed;

            yield return null;
        }

        characterSprite.localPosition = originalCharSpritePos;
        thoughtBubble.localScale = originalThoughtbubbleSize;

        // hole disappears
        spriteMask_1.SetActive(false);
        progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * speed;
            holeSprite.localScale = holeMaxSize - holeMaxSize * progress;
            yield return null;
        }

        holeSprite.localScale = Vector3.zero;

        if (GameState.gameOver == false || charScript.onPlanet_A) charScript.SetMoving(true);
        animating = false;

        yield break;
    }
}