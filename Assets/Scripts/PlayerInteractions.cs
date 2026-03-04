using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            GameManager.Instance.Lose();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Pickup")) return;

        int amount = 1;
        var pv = other.GetComponent<PickupValue>();
        if (pv != null) amount = pv.value;

        GameManager.Instance.AddScore(amount);
        Destroy(other.gameObject);
    }
}