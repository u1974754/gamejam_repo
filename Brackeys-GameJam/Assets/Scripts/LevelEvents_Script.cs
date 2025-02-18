using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level2Events_Scripts : MonoBehaviour
{
    Collider2D collider;
    public GameObject Ceiling;
    public GameObject TrashCan;
    public GameObject Player;

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

    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(levelNumber == 2) StartCoroutine(MoveTheTrashCanUp());
            else if(levelNumber == 3) MakeTheCeilingFall();
        }
    }

    IEnumerator MoveTheTrashCanUp()
    {
        TrashCan.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        Vector2 startPosition = TrashCan.transform.position;
        Vector2 endPosition = startPosition + new Vector2(0, 4);
        float duration = 2.0f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            TrashCan.transform.position = Vector2.Lerp(startPosition, endPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        TrashCan.transform.position = endPosition;
        player_script.setPlayerMovible(false);
        player_script.SetThoughts(new List<string> { "Oh Sh*t!" });
        yield return new WaitForSeconds(2);
        TrashCan.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        TrashCan.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        player_script.setPlayerMovible(true);
    }

    void MakeTheCeilingFall()
    {
        collider.enabled = false;
        Ceiling.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

}
