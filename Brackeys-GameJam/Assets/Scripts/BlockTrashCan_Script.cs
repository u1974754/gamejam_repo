using UnityEngine;
using System.Collections;

public class BlockTrashCan_Script : MonoBehaviour
{

    public GameObject Blocker;

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Blocker.SetActive(true);
            StartCoroutine(MoveTheBlocker());
        }
    }

    IEnumerator MoveTheBlocker()
    {
        Vector3 startPosition = Blocker.transform.position;
        Vector3 endPosition = new Vector3(18.11f, startPosition.y, startPosition.z);
        float elapsedTime = 0f;
        float duration = 0.75f;

        while (elapsedTime < duration)
        {
            Blocker.transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Blocker.transform.position = endPosition;
        yield return new WaitForSeconds(1.0f);
        Blocker.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

}
