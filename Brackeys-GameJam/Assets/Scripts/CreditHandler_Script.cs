using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class CreditHandler_Script : MonoBehaviour
{
    public TextMeshProUGUI creditsText;
    public GameObject SceneManager;

    void Start(){
        string text = creditsText.text;
        StartCoroutine(ShowCredits(new List<string> { text, "Thanks for playing! \nWe hope you enjoyed our first Game Jam project :D" }));
    }

    IEnumerator ShowCredits(List<string> thoughts)
    {
        creditsText.text = "";
        creditsText.gameObject.SetActive(true);

        foreach (string thought in thoughts)
        {
            creditsText.text = "";
            while (creditsText.text.Length < thought.Length)
            {
                creditsText.text += thought[creditsText.text.Length];
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(2);
        }
        creditsText.gameObject.SetActive(false);
        SceneManager.GetComponent<SceneManager_Script>().ChangeToMainMenu();
    }

}
