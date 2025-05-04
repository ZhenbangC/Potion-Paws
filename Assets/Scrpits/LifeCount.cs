using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCount : MonoBehaviour
{
    [Header("UI 显示的生命图标")]
    public Image[] lives;

    [Header("初始生命数量")]
    public int maxLives = 3;
    [HideInInspector] public int livesRemaining;

    private void Start()
    {
        ResetLives(); // 初始设置
    }

    /// <summary>
    /// 玩家失去一条命
    /// </summary>
    public void LoseLife()
    {
        if (livesRemaining == 0)
            return;

        livesRemaining--;
        lives[livesRemaining].enabled = false;

        if (livesRemaining == 0)
        {
            Playermovement player = FindObjectOfType<Playermovement>();
            if (player != null)
            {
                player.Die(); // 玩家死亡触发逻辑
            }
        }
    }

    /// <summary>
    /// 重置生命到最大值（用于复活）
    /// </summary>
    public void ResetLives()
    {
        livesRemaining = maxLives;

        for (int i = 0; i < lives.Length; i++)
        {
            lives[i].enabled = i < maxLives; // 根据最大生命显示
        }
    }
}