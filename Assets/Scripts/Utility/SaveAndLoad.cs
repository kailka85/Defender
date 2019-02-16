using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveAndLoad
{
    public static GameData LoadGameData(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            FileStream file = File.Open(filePath, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            GameData gameData = (GameData)bf.Deserialize(file);
            file.Close();

            return gameData;
        }
        else
        {
            var gameData = new GameData();
            return gameData;
        }
    }

    public static void SaveGameData(GameData gameData, string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        FileStream file = File.Create(filePath);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, gameData);
        file.Close();
    }
}
