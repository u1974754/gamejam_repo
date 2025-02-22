using UnityEngine;
using System.Collections.Generic;

public class Talking_script : MonoBehaviour
{
    public GameObject Racoon;
    private Player Racoon_Methods;
    [SerializeField] private int levelNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Racoon_Methods = Racoon.GetComponent<Player>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(levelNumber == 4) Racoon_Methods.SetThoughts(new List<string> { "What am I suposed to do now?¿?", "...", "It must be something on those bricks..." });
            else if (levelNumber == 5) Racoon_Methods.SetThoughts(new List<string> { "Whats wrong with this guy!?" });
        }
    }

}
