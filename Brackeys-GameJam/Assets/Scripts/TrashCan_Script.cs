using UnityEngine;
using System.Collections;

public class TrashCan_Script : MonoBehaviour
{
    public GameObject SceneManager;
    [SerializeField] private float zoomTime = 2f; // Tiempo total del efecto de zoom
    [SerializeField] private float targetZoomSize = 5f; // Tamaño de zoom deseado
    [SerializeField] private Vector3 targetCameraPositionOffset = new Vector3(0, 0, -1); // Offset de la posición de la cámara

    private bool isZooming = false; // Controla si el zoom está en progreso

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isZooming) // Asegúrate de que el personaje tenga el tag "Player"
        {
            Debug.Log("El personaje ha entrado en el área del trigger.");
            other.GetComponent<Player>().setPlayerMovible(false);
            other.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            StartCoroutine(ZoomTheCamera(other.transform.position)); // Inicia el efecto de zoom
        }
    }

    private IEnumerator ZoomTheCamera(Vector3 targetPosition)
    {
        isZooming = true;

        Camera mainCamera = Camera.main;
        float initialZoomSize = mainCamera.orthographicSize;
        Vector3 initialCameraPosition = mainCamera.transform.position;

        // Ajusta la posición objetivo de la cámara
        Vector3 targetCameraPosition = targetPosition + targetCameraPositionOffset;

        float elapsedTime = 0f;

        while (elapsedTime < zoomTime)
        {
            elapsedTime += Time.deltaTime;

            // Interpola el tamaño de la cámara
            mainCamera.orthographicSize = Mathf.Lerp(initialZoomSize, targetZoomSize, elapsedTime / zoomTime);

            // Interpola la posición de la cámara
            mainCamera.transform.position = Vector3.Lerp(initialCameraPosition, targetCameraPosition, elapsedTime / zoomTime);

            yield return null; // Espera al siguiente frame
        }

        // Asegúrate de que la cámara termine en el estado deseado
        mainCamera.orthographicSize = targetZoomSize;
        mainCamera.transform.position = targetCameraPosition;

        // Cambia a la siguiente escena
        SceneManager.GetComponent<SceneManager_Script>().LoadNextLevel();
    }
}