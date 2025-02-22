using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float pushForce = 5.0f; // Fuerza del mapachito

    [SerializeField] private float zoomTime = 2f; // Tiempo total del efecto de zoom
    [SerializeField] private float targetZoomSize = 5f; // Tamaño de zoom deseado

    private RopeSegment ropeGrabbed = null;
    private RopeSegment lastRopeGrabbed = null;
    private float cooldownAttachSameRope = 0f;
    private float timeAttachSameRope = 0.5f;

    private bool movible = true;
    private bool isGrounded = false;
    private Rigidbody2D rb;
    private Animator animator;
    private bool lookingRight = true;
    private float idleTimer = 0f;
    private Collider2D collider;

    public TextMeshProUGUI thoughtsText;
    public List<string> thoughtTexts;
    public bool dead;
    public RopeSegment fallingRope;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        StartCoroutine(ZoomOutFromPlayer());
        StartCoroutine(enterOntoScene());
        StartCoroutine(ShowThoughts(thoughtTexts));
    }

    void Update()
    {
        CooldownRopeAttachment();
        HandleInput();
        isGrounded = CheckIfGrounded();
        InfoForTheAnimator();
        UpdateIdleTimer();
    }

    void HandleInput()
    {
        if (ropeGrabbed != null)
        {
            animator.SetBool("isSwinging", true);
            AttachToRope(ropeGrabbed);
            float moveHorizontal = Input.GetAxis("Horizontal");

            if ((moveHorizontal > 0 && !lookingRight) || (moveHorizontal < 0 && lookingRight))
            {
                FlipCharacter();
            }

            ropeGrabbed.ApplyHoritzontalForce(moveHorizontal);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("isSwinging", false);
                DeAttachRope(ropeGrabbed);
            }
        }
        else if (!dead)
        {
            if (movible)
            {
                float moveHorizontal = Input.GetAxis("Horizontal");

                if ((moveHorizontal > 0 && !lookingRight) || (moveHorizontal < 0 && lookingRight))
                {
                    FlipCharacter();
                }

                Vector2 movement = new Vector2(moveHorizontal, 0);
                Move(movement);

                if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    Jump();
                }
            }
        }
        else{
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject.Find("SceneManager").GetComponent<SceneManager_Script>().RestartLevel();
            }
        }
    }

    private bool CheckIfGrounded()
    {
        return Mathf.Abs(rb.linearVelocity.y) < 0.1f;
    }

    void InfoForTheAnimator()
    {
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("isJumping", !isGrounded);
    }

    void Move(Vector2 movement)
    {
        animator.SetFloat("xVelocity", Mathf.Abs(movement.x));
        rb.linearVelocity = new Vector2(movement.x * speed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }
    }

    public bool isPlayerMovible()
    {
        return movible;
    }

    public void setPlayerMovible(bool movible)
    {
        rb.linearVelocity = Vector2.zero;
        this.movible = movible;
    }

    private IEnumerator enterOntoScene()
    {
        setPlayerMovible(false);
        animator.SetFloat("xVelocity", 1);
        Vector3 startPosition = new Vector3(-22, transform.position.y, transform.position.z);
        Vector3 endPosition = transform.position;
        transform.position = startPosition;

        float elapsedTime = 0;
        float duration = 2.0f; // Duration of the walk-in animation

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        animator.SetFloat("xVelocity", 0);

        transform.position = endPosition;
        yield return new WaitForSeconds(2);
    }

    IEnumerator ShowThoughts(List<string> thoughts)
    {
        thoughtsText.text = "";
        thoughtsText.gameObject.SetActive(true);

        foreach (string thought in thoughts)
        {
            setPlayerMovible(false);
            thoughtsText.text = "";
            while (thoughtsText.text.Length < thought.Length)
            {
                thoughtsText.text += thought[thoughtsText.text.Length];
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(1);
        }
        setPlayerMovible(true);
        thoughtsText.gameObject.SetActive(false);
    }

    private IEnumerator ZoomOutFromPlayer()
    {
        Camera mainCamera = Camera.main;

        float initialZoomSize = mainCamera.orthographicSize;
        Vector3 initialCameraPosition = mainCamera.transform.position;

        float targetZoom = initialZoomSize;
        Vector3 targetPosition = initialCameraPosition;

        mainCamera.orthographicSize = targetZoomSize;
        mainCamera.transform.position = transform.position + new Vector3(0, 0, -10);

        float elapsedTime = 0f;

        while (elapsedTime < zoomTime)
        {
            elapsedTime += Time.deltaTime;

            mainCamera.orthographicSize = Mathf.Lerp(targetZoomSize, targetZoom, elapsedTime / zoomTime);
            mainCamera.transform.position = Vector3.Lerp(transform.position + new Vector3(0, 0, -10), targetPosition, elapsedTime / zoomTime);

            yield return null;
        }

        mainCamera.orthographicSize = targetZoom;
        mainCamera.transform.position = targetPosition;
    }

    private void FlipCharacter()
    {
        lookingRight = !lookingRight;
        Vector3 scale = transform.localScale;
        scale.y *= -1;
        transform.localScale = scale;
    }

    private void UpdateIdleTimer()
    {
        if (movible && Mathf.Abs(rb.linearVelocity.x) < 0.1f && isGrounded)
        {
            idleTimer += Time.deltaTime;
        }
        else
        {
            idleTimer = 0f;
        }

        animator.SetFloat("timeOnIdle", idleTimer);
    }

    public void SetThoughts(List<string> thoughts)
    {
        thoughtTexts = thoughts;
        StartCoroutine(ShowThoughts(thoughtTexts));
    }

    public void HitByRope(RopeSegment rope)
    {
        if (CanAttachToRope(rope)) AttachToRope(rope);
    }

    private bool CanAttachToRope(RopeSegment rope)
    {
        return lastRopeGrabbed == null || (ropeGrabbed == null && (lastRopeGrabbed.ropeOriginAnchor != rope.ropeOriginAnchor || cooldownAttachSameRope == 0));
    }
    private void AttachToRope(RopeSegment rope)
    {
        transform.position = new Vector3(rope.transform.position.x, rope.transform.position.y, rope.transform.position.z + 1);
        ropeGrabbed = rope;
        lastRopeGrabbed = rope;

       
        if (rope.ropeOriginAnchor != null)
        {
            Rigidbody2D anchorRb = rope.ropeOriginAnchor.GetComponent<Rigidbody2D>();
            if (anchorRb != null)
            {
                anchorRb.bodyType = RigidbodyType2D.Dynamic;
                anchorRb.gravityScale = 1;
                anchorRb.constraints = RigidbodyConstraints2D.None;
            }
        }
    }
    
    private void DeAttachRope(RopeSegment rope)
    {
        if (ropeGrabbed != null)
        {
            Vector2 segmentVelocity = ropeGrabbed.GetComponent<Rigidbody2D>().linearVelocity;
            rb.linearVelocity = segmentVelocity * 0.3f;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, rope.transform.position.z - 1);
        ropeGrabbed = null;
        cooldownAttachSameRope = timeAttachSameRope;
        lastRopeGrabbed = rope;
    }

    private void CooldownRopeAttachment()
    {
        if (cooldownAttachSameRope > 0) cooldownAttachSameRope = Mathf.Max(cooldownAttachSameRope - Time.deltaTime, 0);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Empujable")) 
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                float inputX = Input.GetAxis("Horizontal"); 
                rb.linearVelocity = new Vector2(inputX * pushForce, rb.linearVelocity.y);
            }
        }
    }

    public void MakeItEat()
    {
        int randomEatingTrigger = Random.Range(1, 3); // Generates a random number between 1 and 2
        animator.SetTrigger("Eating" + randomEatingTrigger);
        Debug.Log("Eating" + randomEatingTrigger);
        animator.SetBool("isJumping", false);
        animator.SetBool("isEating", true);
    }
}