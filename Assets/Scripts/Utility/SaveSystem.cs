using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    // Constants
    private static readonly string Extension = ".pseudo";  
    private static readonly string GameDataFilename = "/gdat";  
    
    
    // Should think of better function call (maybe instance containing data)
    public static void SaveGameData(PersistentGameData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + GameDataFilename + Extension;
        FileStream stream = new FileStream(path,FileMode.Create);
        formatter.Serialize(stream,data);
        stream.Close();
    }


    public static PersistentGameData LoadGameData()
    {
        string path = Application.persistentDataPath + GameDataFilename + Extension;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path,FileMode.Open);
            PersistentGameData data =  formatter.Deserialize(stream) as PersistentGameData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}
