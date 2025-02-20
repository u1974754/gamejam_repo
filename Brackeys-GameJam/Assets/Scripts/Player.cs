using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;

    [SerializeField] private float zoomTime = 2f; // Tiempo total del efecto de zoom
    [SerializeField] private float targetZoomSize = 5f; // Tamaño de zoom deseado

    [SerializeField] private LayerMask wallLayer; // LayerMask for the wall

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
    private bool isTouchingWall;

    public TextMeshProUGUI thoughtsText;
    public List<string> thoughtTexts;

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
        isTouchingWall = CheckIfTouchingWall();
    }

    void HandleInput()
    {
        if (ropeGrabbed != null)
        {
            AttachToRope(ropeGrabbed);
            float moveHorizontal = Input.GetAxis("Horizontal");

            if ((moveHorizontal > 0 && !lookingRight) || (moveHorizontal < 0 && lookingRight))
            {
                FlipCharacter();
            }

            ropeGrabbed.ApplyHoritzontalForce(moveHorizontal);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                DeAttachRope(ropeGrabbed);
            }
        }
        else
        {
            if (movible)
            {
                float moveHorizontal = Input.GetAxis("Horizontal");

                if ((moveHorizontal > 0 && !lookingRight) || (moveHorizontal < 0 && lookingRight))
                {
                    FlipCharacter();
                }

                if (!isTouchingWall || (isTouchingWall && ((moveHorizontal > 0 && !IsTouchingWallRight()) || (moveHorizontal < 0 && !IsTouchingWallLeft()))))
                {
                    Vector2 movement = new Vector2(moveHorizontal, 0);
                    Move(movement);
                }

                if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    Jump();
                }
            }
        }

    }

    private bool CheckIfGrounded()
    {
        return Mathf.Abs(rb.linearVelocity.y) < 0.01f;
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
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
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

    private bool CheckIfTouchingWall()
    {
        return rb.IsTouchingLayers(wallLayer);
    }

    private bool IsTouchingWallRight()
    {
        Vector2 direction = Vector2.right;
        return rb.IsTouchingLayers(wallLayer) && Physics2D.Raycast(transform.position, direction, 0.1f, wallLayer);
    }

    private bool IsTouchingWallLeft()
    {
        Vector2 direction = Vector2.left;
        return rb.IsTouchingLayers(wallLayer) && Physics2D.Raycast(transform.position, direction, 0.1f, wallLayer);
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
        return lastRopeGrabbed==null || (ropeGrabbed == null && (lastRopeGrabbed.ropeOriginAnchor != rope.ropeOriginAnchor || cooldownAttachSameRope==0));
    }

    private void AttachToRope(RopeSegment rope)
    {
        transform.position = rope.transform.position;
        ropeGrabbed = rope;
        lastRopeGrabbed = rope;
    }

    private void DeAttachRope(RopeSegment rope)
    {
        if (ropeGrabbed != null) 
        {
            Vector2 segmentVelocity = ropeGrabbed.GetComponent<Rigidbody2D>().linearVelocity;
            rb.linearVelocity = segmentVelocity*0.3f;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        ropeGrabbed = null;
        cooldownAttachSameRope = timeAttachSameRope;
        lastRopeGrabbed = rope;
    }

    private void CooldownRopeAttachment()
    {
        if (cooldownAttachSameRope>0) cooldownAttachSameRope = Mathf.Max(cooldownAttachSameRope - Time.deltaTime,0);
    }

}