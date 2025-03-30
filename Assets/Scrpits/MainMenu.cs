using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public void StartGame()
    {
        if (textMeshProUGUI != null)
        {
            textMeshProUGUI.text = "Loading..."; // 修改文本
        }
        SceneManager.LoadScene("Room"); // 切换场景
    }
}
