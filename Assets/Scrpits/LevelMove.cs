using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove : MonoBehaviour
{
    [Header("切换的目标场景 Index")]
    public int sceneBuildIndex;

    [Header("是否需要按键触发切换")]
    public bool useInteractionKey = true;

    [Header("按键提示 UI")]
    public GameObject interactHint;

    private bool playerInRange = false;

    void Update()
    {
        if (useInteractionKey && playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            SwitchScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (!useInteractionKey)
        {
            SwitchScene();
        }
        else
        {
            if (interactHint != null)
                interactHint.SetActive(true); // 显示提示
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (interactHint != null)
            interactHint.SetActive(false); // 隐藏提示
    }

    void SwitchScene()
    {
        Debug.Log("切换到场景 Index：" + sceneBuildIndex);
        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
    }
}