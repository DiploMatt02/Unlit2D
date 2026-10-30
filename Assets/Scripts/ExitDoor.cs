using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private EndScreens endScreens;
    private void OnCollisionEnter2D(Collision2D collision)
{
    KeyCollector collector = collision.gameObject.GetComponent<KeyCollector>();
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
