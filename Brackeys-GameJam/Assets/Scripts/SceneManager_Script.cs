using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManager_Script : MonoBehaviour
{
    public Animator transition;
    [SerializeField] private float levelTransitionTime = 1.0f;
    [SerializeField] private GameObject DeadCanvas;

    public void RestartLevel()
    {
        StartCoroutine(LoadLevelAfterDelay(SceneManager.GetActiveScene().buildIndex));
        HideDeadCanvas();
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevelAfterDelay(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadLevel1()
    {
        StartCoroutine(LoadLevelAfterDelay(1)); // Asegúrate de que "level_1" está en el índice 1 en Build Settings.
    }

    public void HideDeadCanvas()
    {
        DeadCanvas.SetActive(false);
    }

    public void ChangeToDeadCanvas()
    {
        DeadCanvas.SetActive(true);
    }

    IEnumerator LoadLevelAfterDelay(int levelNumber)
    {
        Debug.Log("Cargando nivel " + levelNumber);
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(levelTransitionTime);
        
        SceneManager.LoadScene(levelNumber);
    }
}
