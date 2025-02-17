using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManager_Script : MonoBehaviour
{
    public Animator transition;
    [SerializeField] private float levelTransitionTime = 1.0f;

    public void RestartLevel()
    {
        // Reiniciar la escena
    }

    public void LoadNextLevel()
    {
       StartCoroutine(LoadLevelAfterDelay(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevelAfterDelay(int levelNumber)
    {
        Debug.Log("Cargando nivel " + levelNumber);
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(levelTransitionTime);
        
        SceneManager.LoadScene(levelNumber);
    }
}
