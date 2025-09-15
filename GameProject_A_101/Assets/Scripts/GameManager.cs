using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("게임 상태")]
    public int playerScore = 0;
    public int itemsCollected = 0;

    public static GameManager instance;

    [Header("UI 참조")]
    public Text scoreText;
    public Text itemCountText;
    public Text gameStatusText;
    


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectItem()
    {
        itemsCollected++;
        Debug.Log($"아이템 수집! (총 : {itemsCollected} 개");
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "점수" + playerScore;
        }
        if(itemCountText != null)
        {
            itemCountText.text = "아이템 : " + itemsCollected + "개";
        }
    }
}
