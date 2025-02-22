using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPosition; // Posición donde el jugador aparecerá

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobamos si el objeto que tocó el portal es el jugador
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>(); // Obtenemos el componente del jugador
            if (player != null)
            {
                // Mover al jugador a la nueva posición
                player.transform.position = spawnPosition;
                Debug.Log("Jugador teletransportado al portal.");
            }
        }
    }
}
