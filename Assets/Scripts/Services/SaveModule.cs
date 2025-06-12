using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
public static class SaveModule
{
    private const string FILENAME = "game_data.json";
    private static Data _cachedData; // Кешированные данные

    // Текущие данные (публичное свойство для доступа)
    public static Data CurrentData
    {
        get
        {
            if (_cachedData == null)
                _cachedData = LoadGameData();
            return _cachedData;
        }
    }

    // Инициализация модуля (можно вызвать при старте игры)
    public static void Initialize()
    {
        _cachedData = LoadGameData();
    }

    // Сохраняет текущие данные в файл
    public static void SaveData()
    {
        if (_cachedData == null) return;

        string json = JsonUtility.ToJson(new DataWrapper(_cachedData), true);
        string path = Path.Combine(Application.persistentDataPath, FILENAME);

        try
        {
            File.WriteAllText(path, json);
            Debug.Log($"Game data saved to {path}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game data: {e.Message}");
        }
    }

    // Загружает данные из файла (без кеширования)
    private static Data LoadGameData()
    {
        string path = Path.Combine(Application.persistentDataPath, FILENAME);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"Save file not found at {path}, creating new data");
            return CreateNewGameData();
        }

        try
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<DataWrapper>(json).data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game data: {e.Message}");
            return CreateNewGameData();
        }
    }

    // Создает новые данные по умолчанию
    private static Data CreateNewGameData()
    {
        return new Data(new List<BusinessData>(), new List<UpgradeData>());
    }

    // Обертка для сериализации
    [Serializable]
    private class DataWrapper
    {
        public Data data;

        public DataWrapper(Data data)
        {
            this.data = data;
        }
    }
}

[Serializable]
public class Data
{
    public int Value = 0;
    public List<BusinessData> BusinessList;
    public List<UpgradeData> Upgrades;

    public Data(List<BusinessData> businessList, List<UpgradeData> upgradesList)
    {
        BusinessList = businessList;
        Upgrades = upgradesList;
    }
}

[Serializable]
public class BusinessData
{
    public string KEY_ID;
    public int Level = 0;

    public BusinessData(string key, int level)
    {
        KEY_ID = key;
        Level = level;
    }
}

[Serializable]
public class UpgradeData
{
    public string KEY_ID;
    public bool IsBuyed = false;

    public UpgradeData(string key, bool isBuyed)
    {
        KEY_ID = key;
        IsBuyed = isBuyed;
    }
}