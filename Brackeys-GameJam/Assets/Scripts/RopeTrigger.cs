using UnityEngine;

public class RopeTrigger : MonoBehaviour
{
    public RopeSegment segment;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.HitByRope(segment);
        }
    }
}
