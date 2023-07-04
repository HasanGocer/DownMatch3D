using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSystem : MonoSingleton<FinishSystem>
{
    [Header("Finish_Field")]
    [Space(10)]

    public int deadWalkerCount = 0;

    public void FinishCheck()
    {
        deadWalkerCount++;
        if (GameManager.Instance.gameStat == GameManager.GameStat.start)
            FinishTime();
    }
    public void FinishTime()
    {
        GameManager gameManager = GameManager.Instance;
        Buttons buttons = Buttons.Instance;
        MoneySystem moneySystem = MoneySystem.Instance;
        LevelManager.Instance.LevelCheck();
        buttons.winPanel.SetActive(true);
        gameManager.gameStat = GameManager.GameStat.finish;
        moneySystem.MoneyTextRevork(gameManager.addedMoney);
    }
}
