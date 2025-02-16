using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        handleInput();
    }

    void handleInput()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal, 0);
        
        Move(movement);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void Move(Vector2 movement)
    {
        rb.linearVelocity = new Vector2(movement.x * speed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (Mathf.Abs(rb.linearVelocity.y) < 0.001f) // Check if the player is grounded
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TrashCan"))
        {
            Debug.Log("Player collided with trash can");
        }
    }
}
