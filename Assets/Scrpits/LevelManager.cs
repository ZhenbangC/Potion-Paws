using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Playermovement player;
    private LifeCount lifeCount;

    private void Start()
    {
        player = FindObjectOfType<Playermovement>();
        lifeCount = FindObjectOfType<LifeCount>();

        if (player != null)
        {
            // 初始出生点设为初始位置
            player.SetRespawnPoint(player.transform.position);
        }
    }

    public void Restart()
    {
        if (player != null)
        {
            player.Respawn(); // 将玩家传送回最近的平台
        }

        if (lifeCount != null)
        {
            lifeCount.ResetLives(); // 重置生命数量
        }
    }
}