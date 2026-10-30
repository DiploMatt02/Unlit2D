using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    //Shows the win screen
    [SerializeField] private EndScreens endScreens;
    private void OnCollisionEnter2D(Collision2D collision)
{
    //Only the player can open the door
    KeyCollector collector = collision.gameObject.GetComponent<KeyCollector>();
        if (collector == null)
        {
            return;
        }
        //Win with all keys, otherwise stay locked
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
