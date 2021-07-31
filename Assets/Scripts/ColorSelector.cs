using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelector : MonoBehaviour
{
    ColorWave curColorWave;

    public Material bridgeMat;

    [Header("PlayerParts")]
    public Material pollMat;

    public Material legMat;
    public Material legBaseMat;

    void Awake()
    {
        curColorWave = SavedData.savedData.transform.GetComponent<ColorWavesHolder>().colorWaves[SavedData.savedData.colorWave];
        ApplyColors();
    }

    void ApplyColors()
    {
        pollMat.color = curColorWave.pollMat;
        legMat.color = curColorWave.legsMat;
        legBaseMat.color = curColorWave.playerBaseMat;

        bridgeMat.color = curColorWave.bridgeMat;
    }
}
