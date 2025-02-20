using UnityEngine;

public class RopeSegment : MonoBehaviour
{
    public Transform ropeOriginAnchor;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public void ApplyHoritzontalForce(float direction)
    {
        if (rb != null)
        {
            Vector2 force = new Vector2(direction*0.1f, 0);
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }

}
