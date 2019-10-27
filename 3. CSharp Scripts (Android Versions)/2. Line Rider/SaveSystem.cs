using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    
    public static void SaveLines (GameManager gameManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/lines.omg";
        FileStream stream = new FileStream(path, FileMode.Create);

        LinesData data = new LinesData(gameManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static LinesData LoadLines ()
    {
        string path = Application.persistentDataPath + "/lines.omg";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LinesData data = formatter.Deserialize(stream) as LinesData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file was not found in: " + path);
            return null;
        }
    }
}
