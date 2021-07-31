using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SavedData : MonoBehaviour
{
    public static SavedData savedData;

    public int level;
    public int coins;
    public int colorWave;

    public List<int> purchasedWaves;

    private void Awake()
    {
        if (savedData == null)
        {
            savedData = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (savedData != this)
        {
            Destroy(gameObject);
        }

        //load information as soon as possible
        Load();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")) file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
        else file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        data.level = level;
        data.coins = coins;
        data.colorWave = colorWave;

        data.purchasedWaves = purchasedWaves;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        //File.Delete(Application.persistentDataPath + "/playerInfo.dat");
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            level = data.level;
            coins = data.coins;
            colorWave = data.colorWave;

            purchasedWaves = data.purchasedWaves;
        }

        if (purchasedWaves == null) purchasedWaves = new List<int>();
    }

    [Serializable]
    class PlayerData
    {
        public int level;
        public int coins;
        public int colorWave;

        public List<int> purchasedWaves;
    }
}
