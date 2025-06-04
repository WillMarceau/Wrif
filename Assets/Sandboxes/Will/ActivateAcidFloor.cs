using UnityEngine;
using System.Collections;

public class ActivateAcidFloor : MonoBehaviour
{
    private float riseDuration = 3f;
    private float riseHeight = 6f;
    private bool risen = false;
    public GameObject acidFloor;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !risen)
        {
            risen = true;
            acidFloor.SetActive(true);
            StartCoroutine(Rise());
        }
    }

    private IEnumerator Rise()
    {
        Vector3 startPos = acidFloor.transform.position;
        Vector3 endPos = startPos + new Vector3(0, riseHeight, 0);
        float elapsed = 0f;

        while (elapsed < riseDuration)
        {
            acidFloor.transform.position = Vector3.Lerp(startPos, endPos, elapsed / riseDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        acidFloor.transform.position = endPos;
    }

}
