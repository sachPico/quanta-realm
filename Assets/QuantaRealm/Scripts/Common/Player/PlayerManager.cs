using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public TMP_Text livesText;

    public Image gameOverBackground;
    public TMP_Text gameOverText;

    int lives = 2;

    public static int Lives
    {
        get
        {
            return instance.lives;
        }
        set
        {
            instance.lives = value;
            instance.livesText.text = value.ToString();
        }
    }

    public static void StartGameOverSequence()
    {
        instance.StartCoroutine(instance.GameOverSequence());
    }

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        Lives = 2;

        Color c = gameOverBackground.color;
        c.a = 0;
        gameOverBackground.color = c;
        gameOverBackground.gameObject.SetActive(false);

        c = gameOverText.color;
        c.a = 0;
        gameOverText.color = c;
        gameOverText.gameObject.SetActive(false);

        livesText.text = Lives.ToString();
    }

    IEnumerator GameOverSequence()
    {
        gameOverBackground.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);

        while (gameOverBackground.color.a <= 1f)
        {
            Color c = gameOverBackground.color;
            c.a += .01f;
            gameOverBackground.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        while (gameOverText.color.a <= 1f)
        {
            Color c = gameOverText.color;
            c.a += .01f;
            gameOverText.color = c;
            yield return null;
        }

        yield break;
    }
}
