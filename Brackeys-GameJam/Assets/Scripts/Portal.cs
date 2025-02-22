using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPosition; // Posici�n donde el jugador aparecer�

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobamos si el objeto que toc� el portal es el jugador
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>(); // Obtenemos el componente del jugador
            if (player != null)
            {
                // Mover al jugador a la nueva posici�n
                player.transform.position = spawnPosition;
                Debug.Log("Jugador teletransportado al portal.");
            }
        }
    }
}
