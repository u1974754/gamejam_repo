using UnityEngine;

public class Rope : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player=collision.attachedRigidbody.GetComponent<Player>();
        if (player != null)
        {
            player.GrabRope(this);
        }
    }
}
