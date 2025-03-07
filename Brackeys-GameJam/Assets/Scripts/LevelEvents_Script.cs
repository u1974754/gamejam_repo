using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelEvents_Scripts : MonoBehaviour
{
    Collider2D collider;
    public GameObject Ceiling;
    public GameObject TrashCan;
    public GameObject Player;
    public GameObject Brick;
    public GameObject Rope;
    public GameObject FloorBreakable;

    private Player player_script;

    [SerializeField] private int levelNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<Collider2D>();
        player_script = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (levelNumber == 2) StartCoroutine(MoveTheTrashCanUp());
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            else if (levelNumber == 3) MakeTheCeilingFall();
            else if (levelNumber == 4) MakeBrickFall();
            else if (levelNumber == 5) MakeRopeFall();
=======
            else if (levelNumber == 3) StartCoroutine(MakeTheCeilingFall());
            else if (levelNumber == 4 || levelNumber == 5) MakeBrickFall();
            else if (levelNumber == 6 || levelNumber == 7 || levelNumber == 8) MakeRopeAppear();
>>>>>>> Stashed changes
=======
            else if (levelNumber == 3) StartCoroutine(MakeTheCeilingFall());
            else if (levelNumber == 4 || levelNumber == 5) MakeBrickFall();
            else if (levelNumber == 6 || levelNumber == 7 || levelNumber == 8) MakeRopeAppear();
>>>>>>> Stashed changes
        }
    }

    IEnumerator MoveTheTrashCanUp()
    {
        collider.enabled = false;
        TrashCan.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        Vector2 startPosition = TrashCan.transform.position;
        Vector2 endPosition = startPosition + new Vector2(0, 5);
        float duration = 2.0f;
        float elapsed = 0.0f;

        player_script.setPlayerMovible(false);
        player_script.SetThoughts(new List<string> { "Oh Sh*t!" });

        while (elapsed < duration)
        {
            TrashCan.transform.position = Vector2.Lerp(startPosition, endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        TrashCan.transform.position = endPosition;
        TrashCan.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        yield return new WaitForSeconds(2);
        TrashCan.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        player_script.setPlayerMovible(true);
    }

    void MakeTheCeilingFall()
    {
        Debug.Log("MakeTheCeilingFall");
        collider.enabled = false;
        Ceiling.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    void MakeBrickFall()
    {
        Rigidbody2D rb = Brick.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            collider.enabled = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1;
            StartCoroutine(RotateBrick());
        }
    }

    IEnumerator RotateBrick()
    {
        Quaternion startRotation = Brick.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, 90);
        float duration = 1.0f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            Brick.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Brick.transform.rotation = endRotation;
    }

    void MakeRopeFall()
    {
        Rigidbody2D rb = Brick.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            collider.enabled = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1;
            StartCoroutine(RotateBrick());
            StartCoroutine(MoveBrickToPosition(new Vector2(-10, -8)));
        }
    }

    IEnumerator MoveBrickToPosition(Vector2 targetPosition)
    {
        Vector2 startPosition = Brick.transform.position;
        float duration = 1.0f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            Brick.transform.position = Vector2.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Brick.transform.position = targetPosition;

        FloorBreakable.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        yield return new WaitForSeconds(2);
        FloorBreakable.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Rope.SetActive(true);
=======
        if(levelNumber == 6) player_script.SetThoughts(new List<string> { "This isn't Funny anymore" });
        yield return new WaitForSeconds(2);
        FloorBreakable.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Rope.SetActive(true);
        if (levelNumber != 6)Rope_2.SetActive(true);
>>>>>>> Stashed changes
=======
        if(levelNumber == 6) player_script.SetThoughts(new List<string> { "This isn't Funny anymore" });
        yield return new WaitForSeconds(2);
        FloorBreakable.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Rope.SetActive(true);
        if (levelNumber != 6)Rope_2.SetActive(true);
>>>>>>> Stashed changes
    }

}
