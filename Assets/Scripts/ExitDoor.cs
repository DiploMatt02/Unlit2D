using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private EndScreens endScreens;
    private void OnTriggerEnter2D(Collider2D other)
    {
        KeyCollector collector = other.GetComponent<KeyCollector>();
        if (collector == null)
        {
            return;
        }
        if (collector.HasCollectedAllKeys())
        {
            Win();
        }
        else
        {
            Debug.Log("The door is locked. Find all the keys.");
        }
    }

    private void Win()
    {
        endScreens.ShowWin();
    }
}
