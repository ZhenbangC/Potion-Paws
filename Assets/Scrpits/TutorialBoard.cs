using System.Collections;
using UnityEngine;

public class TutorialBoard : MonoBehaviour
{
    [Tooltip("这个是挂在场景中的提示UI（Canvas）")]
    public GameObject billboardCanvas;

    private bool hasTriggered = false;

    private void Start()
    {
        if (billboardCanvas != null)
            billboardCanvas.SetActive(true); // 一开始显示
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(HideAfterDelay(10f)); // 延迟10秒后隐藏
        }
    }

    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (billboardCanvas != null)
        {
            billboardCanvas.SetActive(false);
        }
    }
}