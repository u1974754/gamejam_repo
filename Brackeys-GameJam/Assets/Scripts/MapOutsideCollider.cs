using UnityEngine;

public class MapOutsideCollider : MonoBehaviour
{
    private GameObject Racoon;
    private GameObject SceneManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Racoon = GameObject.Find("Racoon");       
        Debug.Log("Racoon: " + Racoon);

        SceneManager = GameObject.Find("SceneManager"); 
    }

    private void ShowDeadCanvas(){
        SceneManager.GetComponent<SceneManager_Script>().ChangeToDeadCanvas();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Racoon.GetComponent<Player>().setPlayerMovible(false);
            Racoon.GetComponent<Player>().dead = true;
            ShowDeadCanvas();
        }
    }

}
