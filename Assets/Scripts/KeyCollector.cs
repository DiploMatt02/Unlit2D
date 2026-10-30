using UnityEngine;

public class KeyCollector : MonoBehaviour
{
    //Keys needed to open the exit door
    [SerializeField] private int keysRequired = 3;

    //Live state
    private int keysCollected;

    //Called by key pickups
    public void CollectKey()
    {
        keysCollected++;
        Debug.Log("Keys: " + keysCollected + "/" + keysRequired);
    }

    //Exit door asks if it can open
    public bool HasCollectedAllKeys()
    {
        return keysCollected >= keysRequired;
    }

    //HUD asks for the key count
    public int GetKeysCollected()
    {
        return keysCollected;
    }

    public int GetKeysRequired()
    {
        return keysRequired;
    }
}
