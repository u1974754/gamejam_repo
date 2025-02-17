using UnityEngine;
using TMPro;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;

    [SerializeField] private float zoomTime = 2f; // Tiempo total del efecto de zoom
    [SerializeField] private float targetZoomSize = 5f; // Tamaño de zoom deseado

    private bool movible = true;
    private bool isGrounded = false;
    private Rigidbody2D rb;
    private Animator animator;
    private bool lookingRight = true;
    private float idleTimer = 0f;

    public TextMeshProUGUI thoughtsText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(ZoomOutFromPlayer());
        StartCoroutine(enterOntoScene());
        StartCoroutine(ShowThoughts("What a nice day to eat trash..."));
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        isGrounded = CheckIfGrounded();
        InfoForTheAnimator();
        UpdateIdleTimer(); 
    }

    void HandleInput()
    {
        if(movible){
            float moveHorizontal = Input.GetAxis("Horizontal");

            if((moveHorizontal > 0 && !lookingRight) || (moveHorizontal < 0 && lookingRight)){
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

    private bool CheckIfGrounded(){
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
        setPlayerMovible(true);
    }

    IEnumerator ShowThoughts(string thought)
    {
        thoughtsText.text = "";
        thoughtsText.gameObject.SetActive(true);
        
        while (thoughtsText.text.Length < thought.Length)
        {
            thoughtsText.text += thought[thoughtsText.text.Length];
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(2);
        thoughtsText.gameObject.SetActive(false);
    }

    private IEnumerator ZoomOutFromPlayer()
    {
        Camera mainCamera = Camera.main;

        // Store initial camera values
        float initialZoomSize = mainCamera.orthographicSize;
        Vector3 initialCameraPosition = mainCamera.transform.position;

        // Define target zoom size and position
        float targetZoom = initialZoomSize; // The desired zoom size is the initial size
        Vector3 targetPosition = initialCameraPosition; // The desired position is the initial position

        // Set the camera to the player's position and zoom in
        mainCamera.orthographicSize = targetZoomSize;
        mainCamera.transform.position = transform.position + new Vector3(0, 0, -10); // Adjust Z for 2D (usually -10)

        float elapsedTime = 0f;

        while (elapsedTime < zoomTime)
        {
            elapsedTime += Time.deltaTime;

            // Interpolate the camera's orthographic size (zoom out)
            mainCamera.orthographicSize = Mathf.Lerp(targetZoomSize, targetZoom, elapsedTime / zoomTime);

            // Interpolate the camera's position to move back to the initial position
            mainCamera.transform.position = Vector3.Lerp(transform.position + new Vector3(0, 0, -10), targetPosition, elapsedTime / zoomTime);

            yield return null; // Wait for the next frame
        }

        // Ensure the camera ends at the exact target values
        mainCamera.orthographicSize = targetZoom;
        mainCamera.transform.position = targetPosition;
    }

    private void FlipCharacter(){
        lookingRight = !lookingRight;
        Vector3 scale = transform.localScale;
        scale.y *= -1;
        transform.localScale = scale;
    }

    private void UpdateIdleTimer()
    {
        // Si el jugador está en idle (no se mueve y está en el suelo)
        if (movible && Mathf.Abs(rb.linearVelocity.x) < 0.1f && isGrounded)
        {
            idleTimer += Time.deltaTime; // Incrementa el contador de tiempo
        }
        else
        {
            idleTimer = 0f; // Reinicia el contador si el jugador se mueve
        }

        // Actualiza el parámetro en el Animator
        animator.SetFloat("timeOnIdle", idleTimer);
    }
}
