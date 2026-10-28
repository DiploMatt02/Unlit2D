using UnityEngine;

public class KeyCollector : MonoBehaviour
{
    [SerializeField] private int keysRequired = 3;

    private int keysCollected;

    public void CollectKey()
    {
        keysCollected++;
        Debug.Log("Keys: " + keysCollected + "/" + keysRequired);
    }

    public bool HasCollectedAllKeys()
    {
        return keysCollected >= keysRequired;
    }

    public int GetKeysCollected()
    {
        return keysCollected;
    }

    public int GetKeysRequired()
    {
        return keysRequired;
    }
}
