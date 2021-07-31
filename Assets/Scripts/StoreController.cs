using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StoreController : MonoBehaviour
{
    List<ColorWave> colorWaves;

    public GameObject colorWaveButton;
    public GameObject buttonsPanel;

    public TextMeshProUGUI coinsText;

    void Awake()
    {
        colorWaves = SavedData.savedData.transform.GetComponent<ColorWavesHolder>().colorWaves;

        LoadButtons();
    }

    private void Update()
    {
        coinsText.text = SavedData.savedData.coins.ToString();
    }

    void LoadButtons()
    {
        int cost = 0;
        int number = 0;

        foreach (ColorWave colorWave in colorWaves)
        {
            GameObject curColorButton = Instantiate(colorWaveButton, Vector3.zero, Quaternion.identity);
            curColorButton.transform.parent = buttonsPanel.transform;

            ColorWaveButton curColorWaveScript = curColorButton.GetComponent<ColorWaveButton>();

            curColorWaveScript.image1.color = colorWave.legsMat;
            curColorWaveScript.image2.color = colorWave.playerBaseMat;
            curColorWaveScript.image3.color = colorWave.bridgeMat;
            //if wave has been purchased, check by seeing if number is in saved list, set cost to zero
            if (SavedData.savedData.purchasedWaves.Contains(number))
            {
                curColorWaveScript.cost = 0;
                curColorWaveScript.costText.text = 0.ToString();
            }
            else
            {
                curColorWaveScript.cost = cost;
                curColorWaveScript.costText.text = cost.ToString();
            }

            curColorWaveScript.number = number;

            curColorWaveScript.controller = this;

            cost += 10;
            number++;
        }
    }

    public void BuyColorWave(int waveNumber, int price)
    {
        if (SavedData.savedData.coins >= price)
        {
            AudioManager.instance.Play("Click");

            SavedData.savedData.coins -= price;
            SavedData.savedData.colorWave = waveNumber;

            //If wave number hasnt been purchased before, Add number of purchsed wave to saved list
            if (!SavedData.savedData.purchasedWaves.Contains(waveNumber)) SavedData.savedData.purchasedWaves.Add(waveNumber);

            SavedData.savedData.Save();

            BackButton();
        }
    }

    public void BackButton()
    {
        AudioManager.instance.Play("Click");

        SceneManager.LoadScene("GameScene");
    }
}
