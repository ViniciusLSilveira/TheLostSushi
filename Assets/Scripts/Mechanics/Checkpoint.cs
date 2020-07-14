using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            collision.GetComponent<Player>().SetLastCheckPoint(this.transform);
            StateManager.Instance.Save();
        }
    }
}
