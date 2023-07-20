using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishSystem : MonoSingleton<FinishSystem>
{
    public void CheckFail()
    {
        if (GameManager.Instance.gameStat == GameManager.GameStat.start && CounterSystem.Instance.counterCount <= 0)
            Buttons.Instance.failPanel.SetActive(true);
    }

    public void FinishCheck()
    {
        if (GameManager.Instance.gameStat == GameManager.GameStat.start)
            FinishTime();
    }
    private void FinishTime()
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
