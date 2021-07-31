using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    int level = 0;

    LevelGenerator levelGenerator;

    private void Awake()
    {
        levelGenerator = this.GetComponent<LevelGenerator>();
    }

    void Start()
    {
        level = SavedData.savedData.level;
        NextLevel();
    }

    void NextLevel()
    {
        if (level <= 2) levelGenerator.GenerateLevel(2, 10, 1);
        else if (level <= 5) levelGenerator.GenerateLevel(3, 12, 2);
        else if (level <= 8) levelGenerator.GenerateLevel(4, 14, 4);
        else if (level <= 13) levelGenerator.GenerateLevel(5, 15, 5);
        else if (level <= 17) levelGenerator.GenerateLevel(5, 15, 7);
        else levelGenerator.GenerateLevel(5, 15, 8);
    }
}
