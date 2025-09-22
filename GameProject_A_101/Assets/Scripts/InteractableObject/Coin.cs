using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : InteractableObject
{
    [Header("동전 설정")]
    public int coinValue = 10;

    protected override void Start()
    {
        base.Start();
        objectName = "동전";
        interactionText = "[E] 동전 획득";
        interactionType = InteractionType.Item;
    }

    protected override void CollectItem()
    {
        transform.Rotate(Vector3.up, 360f);
        Destroy(gameObject, 0.5f);
    }
}
