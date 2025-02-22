using UnityEngine;
using System.Collections;

public class LastLevel_Script : MonoBehaviour
{
    public GameObject SceneManager;
    [SerializeField] private float zoomTime = 2f; // Tiempo total del efecto de zoom
    [SerializeField] private float targetZoomSize = 5f; // Tamaño de zoom deseado

    private bool isZooming = false; // Controla si el zoom está en progreso

    void Start()
    {
        SceneManager = GameObject.Find("SceneManager");
        SceneManager.GetComponent<SceneManager_Script>().HideDeadCanvas();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isZooming) // Asegúrate de que el personaje tenga el tag "Player"
        {
            other.GetComponent<Player>().setPlayerMovible(false);
            other.GetComponent<Player>().MakeItEat();
            other.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            StartCoroutine(ZoomTheCamera(other.transform)); // Inicia el efecto de zoom
        }
    }

    private IEnumerator ZoomTheCamera(Transform playerTransform)
    {
        isZooming = true;

        Camera mainCamera = Camera.main;
        float initialZoomSize = mainCamera.orthographicSize;
        Vector3 initialCameraPosition = mainCamera.transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < zoomTime)
        {
            elapsedTime += Time.deltaTime;

            // Interpola el tamaño de la cámara
            mainCamera.orthographicSize = Mathf.Lerp(initialZoomSize, targetZoomSize, elapsedTime / zoomTime);

            // Interpola la posición de la cámara
            mainCamera.transform.position = Vector3.Lerp(initialCameraPosition, playerTransform.position, elapsedTime / zoomTime);

            yield return null; // Espera al siguiente frame
        }

        // Asegúrate de que la cámara termine en el estado deseado
        mainCamera.orthographicSize = targetZoomSize;
        mainCamera.transform.position = playerTransform.position;

        // Cambia a la siguiente escena
        SceneManager.GetComponent<SceneManager_Script>().LoadNextLevel();
    }
}
