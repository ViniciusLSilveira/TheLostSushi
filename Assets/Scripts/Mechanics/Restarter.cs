using UnityEngine;
public class Restarter : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<Player>().TakeDamage(1);
        }
    }
}
