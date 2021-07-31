using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    public PlayerMovementController playerMovement;
    public PlayerController playerController;

    public static bool firstPlay = true;
    public static int timesPlayed = 0;

    [Header("BasePanel")]
    public GameObject basePanel;
    public TextMeshProUGUI title;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI playText;
    public GameObject leaderboardsButton;
    public GameObject shopButton;
    public GameObject gemImage;
    public TextMeshProUGUI gemsText;
    public GameObject soundsButton;
    public GameObject musicButton;

    public static bool soundOn;
    static bool musicOn;
    Color onColor = Color.white;
    Color offColor = Color.gray;

    public Button TapToPlayButton;

    string playString = "Tap To Play";

    private void Awake()
    {
        print(firstPlay);
        if (firstPlay)
        {
            soundOn = true;
            musicOn = true;

            OpenBasePanel();
        }
        else
        {
            basePanel.SetActive(false);
            playerMovement.StartPlaying();
        }
    }

    public void TapToPlay()
    {
        AudioManager.instance.Play("Click");

        if (firstPlay)
        {
            CloseBasePanel();
            playerMovement.StartPlaying();

            firstPlay = false;
            print("First");
        }
        else
        {
            print("again");
            SceneManager.LoadScene("GameScene");
        }
    }

    public void OpenBasePanel()
    {
        if (firstPlay) playString = "Tap To Play";
        else playString = "Tap To Play Again";

        playerMovement.canMove = false;

        SavedData.savedData.Save();

        levelText.text = SavedData.savedData.level.ToString();
        gemsText.text = SavedData.savedData.coins.ToString();
        playText.text = playString;

        basePanel.GetComponent<Image>().color = new Color(1,1,1, 0);
        basePanel.SetActive(true);

        title.color = new Color(1, 1, 1, 0);
        playText.color = new Color(1, 1, 1, 0);
        levelText.color = new Color(1, 1, 1, 0);
        leaderboardsButton.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        shopButton.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        gemImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        gemsText.color = new Color(1, 1, 1, 0);

        soundsButton.SetActive(true);
        musicButton.SetActive(true);

        if (soundOn) soundsButton.GetComponent<Image>().color = onColor;
        else soundsButton.GetComponent<Image>().color = offColor;
        print("Sound: " + soundOn);
        if (musicOn) musicButton.GetComponent<Image>().color = onColor;
        else musicButton.GetComponent<Image>().color = offColor;

        StartCoroutine(FadeTo(basePanel, .45f, 1.3f));
        StartCoroutine(FadeTextTo(title, 1f, 1.3f));
        StartCoroutine(FadeTextTo(playText, 1f, 1.3f));
        StartCoroutine(FadeTextTo(levelText, 1f, 1.3f));
        StartCoroutine(FadeTo(leaderboardsButton, 1f, 1.3f));
        StartCoroutine(FadeTo(shopButton, 1f, 1.3f));
        StartCoroutine(FadeTo(gemImage, 1f, 1.3f));
        StartCoroutine(FadeTextTo(gemsText, 1f, 1.3f));

        //StartCoroutine(FadeTo(soundsButton, 1f, 1.3f));
        //StartCoroutine(FadeTo(musicButton, 1f, 1.3f));

        TapToPlayButton.interactable = true;

        if (timesPlayed == 6)
        {
            AdController.AdInstance.ShowAd("video");
        }
        else if (timesPlayed == 12)
        {
            AdController.AdInstance.ShowAd("rewardedVideo");
            timesPlayed = 0;
        }

        AudioManager.instance.Stop("RailSound");
    }

    public void CloseBasePanel()
    {
        TapToPlayButton.interactable = false;
        StartCoroutine(FadeTo(basePanel, 0, 1.3f));
        StartCoroutine(FadeTextTo(title, 0, 1.3f));
        StartCoroutine(FadeTextTo(playText, 0, 1.3f));
        StartCoroutine(FadeTextTo(levelText, 0f, 1.3f));
        StartCoroutine(FadeTo(leaderboardsButton, 0f, 1.3f));
        StartCoroutine(FadeTo(shopButton, 0f, 1.3f));
        StartCoroutine(FadeTo(gemImage, 0f, 1.3f));
        StartCoroutine(FadeTextTo(gemsText, 0f, 1.3f));

        soundsButton.SetActive(false);
        musicButton.SetActive(false);

        //StartCoroutine(FadeTo(soundsButton, 0f, 1.3f));
        //StartCoroutine(FadeTo(musicButton, 0f, 1.3f));
    }

    IEnumerator FadeTo(GameObject curObject, float aValue, float aTime)
    {
        float alpha = curObject.GetComponent<Image>().color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            curObject.GetComponent<Image>().color = newColor;
            yield return null;
        }

        if (curObject == basePanel && aValue == 0) curObject.SetActive(false);
    }

    IEnumerator FadeTextTo(TextMeshProUGUI curText, float aValue, float aTime)
    {
        float alpha = curText.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
            curText.color = newColor;
            yield return null;
        }
    }

    public void ShopButton()
    {
        AudioManager.instance.Play("Click");
        SceneManager.LoadScene("ShopScene");
    }

    public void SoundButton()
    {
        if (soundOn) soundOn = false;
        else soundOn = true;

        if (soundOn) soundsButton.GetComponent<Image>().color = onColor;
        else soundsButton.GetComponent<Image>().color = offColor;
    }

    public void MusicButton()
    {
        if (musicOn) musicOn = false;
        else musicOn = true;

        if (musicOn) musicButton.GetComponent<Image>().color = onColor;
        else musicButton.GetComponent<Image>().color = offColor;
    }

}
