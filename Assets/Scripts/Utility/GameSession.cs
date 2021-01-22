using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;
    // Current Game Data
    [CanBeNull] private PersistentGameData _data;

    [CanBeNull]
    public PersistentGameData Data => _data;
    
    // Start is called before the first frame update
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            _data = SaveSystem.LoadGameData();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool SaveData(Vector3 pos, int level)
    {
        PersistentGameData dat = new PersistentGameData(pos,level);
        if (_data == dat) return false;
        SaveSystem.SaveGameData(dat);
        _data = new PersistentGameData(pos,level);
        return true;
    }

    public void ReloadSceneFromCheckpoint()
    {
        SectionManager.instance.loader.ResetScene();
    }
}
