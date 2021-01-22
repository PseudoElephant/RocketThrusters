using UnityEngine;

[System.Serializable]
public class PersistentGameData
{
    public int level;
    public float[] playerPosition;

    
    // Constructor
    public PersistentGameData(float[] position, int level)
    {
        playerPosition = position;
        this.level = level;
    }
    
    public PersistentGameData(Vector3 position, int level)
    {
        playerPosition = new float[3];
        for (int i = 0; i < 3; i++)
            playerPosition[i] = position[i];
        this.level = level;
    }
    
}