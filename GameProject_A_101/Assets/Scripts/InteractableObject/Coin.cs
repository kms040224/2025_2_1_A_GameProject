using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : InteractableObject
{
    [Header("동전 설정")]
    public int coinValue = 10;
    public string questTag = "Coin";

    protected override void Start()
    {
        base.Start();
        objectName = "동전";
        interactionText = "[E] 동전 획득";
        interactionType = InteractionType.Item;
    }

    protected override void CollectItem()
    {

        //퀘스트 매니저에 수집을 알림
        if(QuestManager.instance != null)
        {
            QuestManager.instance.AddCollectProgress(questTag);
        }

        AchievementManager.instance?.UpdateProgress(AchievementType.CollectCoins, coinValue);

        transform.Rotate(Vector3.up, 360f);
        Destroy(gameObject, 0.5f);
    }
}
