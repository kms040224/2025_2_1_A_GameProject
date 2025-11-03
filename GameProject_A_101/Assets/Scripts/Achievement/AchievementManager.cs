using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{

    public static AchievementManager instance;

    [Header("Achievement Settings")]
    public List<AchievementData> allAchievements = new List<AchievementData>();

    [Header("UI References")]
    public GameObject achievementPopUpPrefab;
    public Transform popUpParent;
    public GameObject achievementPanel;
    public Transform achievementListContent;
    public GameObject achievementSlotPrefab;

    private Dictionary<AchievementType, int> progressData = new Dictionary<AchievementType, int>();     //통계 저장

    void Awake()
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
    // Start is called before the first frame update
    void Start()
    {
        ResetAllAchievements();                         //시작시에 리셋 강제로 (테스트용) 나중에 배포시에는 삭제
        foreach(AchievementType type in System.Enum.GetValues(typeof(AchievementType)))
        {
            progressData[type] = 0;
        }
        LoadAchievements();
        UpdateAchievementUI();
    }

    

    public void UpdateAchievementUI()
    {
        if(achievementListContent == null || achievementSlotPrefab == null)
        {
            return;
        }

        foreach(Transform child in achievementListContent)
        {
            Destroy(child.gameObject);
        }

        foreach(AchievementData achievement in allAchievements)
        {
            GameObject slot = Instantiate(achievementSlotPrefab, achievementListContent);
            AchievementSlot slotScript = slot.GetComponent<AchievementSlot>();
            if(slotScript != null)
            {
                slotScript.SetAchievement(achievement, GetProgress(achievement));
            }
        }
    }


    public void UpdateProgress(AchievementType type, int amount = 1)
    {
        progressData[type] += amount;


        //해당 타입의 모든 업적 체크
        foreach(AchievementData achievement in allAchievements)
        {
            if (achievement.achievementType == type && !achievement.isUnlocked)
            {
                if(progressData[type] >= achievement.requiredAmount)
                {
                    UnlockAchievement(achievement);
                }
            }
        }
    }

    void UnlockAchievement(AchievementData achievement)
    {
        achievement.isUnlocked = true;
        //보상이 있는 업적일 경우 보상 로직을 여기에 넣는다.
        ShowAchievementPopup(achievement);
        UpdateAchievementUI();
    }

    void ShowAchievementPopup(AchievementData achievement)
    {
        if(achievementPopUpPrefab != null && popUpParent != null)
        {
            GameObject popup = Instantiate(achievementPopUpPrefab, popUpParent);

            Text titleText = popup.transform.Find("Title")?.GetComponent<Text>();
            Text descText = popup.transform.Find("Description")?.GetComponent<Text>();
        }
    }

    public float GetProgress(AchievementData achievement)
    {
        if (achievement.isUnlocked) return 1f;
        int current = progressData.ContainsKey(achievement.achievementType) ? progressData[achievement.achievementType] : 0;
        return Mathf.Min((float)current / achievement.requiredAmount, 1f);
    }

    void SaveAchievements()
    {
        foreach (var kvp in progressData)
        {
            PlayerPrefs.SetInt("Achievement_" + kvp.Key, kvp.Value);
        }
        foreach (AchievementData achievement in allAchievements)
        {
            PlayerPrefs.SetInt("Unlocked_" + achievement.name, achievement.isUnlocked ? 1 : 0);
        }
    }
    void LoadAchievements()
    {
        foreach (AchievementType type in System.Enum.GetValues(typeof(AchievementType)))
        {
            progressData[type] = PlayerPrefs.GetInt("Achievement_" +  type, 0);
        }
        foreach (AchievementData achievement in allAchievements)
        {
            achievement.isUnlocked = PlayerPrefs.GetInt("Unlocked_" + achievement.name, 0) == 1;
        }
    }

    public void ResetAllAchievements()
    {
        foreach (AchievementType type in System.Enum.GetValues (typeof(AchievementType)))
        {
            progressData[type] = 0;
            PlayerPrefs.DeleteKey("Achievement_" + type);
        }

        foreach (AchievementData achievement in allAchievements)
        {
            achievement.isUnlocked = false;
            PlayerPrefs.DeleteKey("Unlocked_" +  achievement.name);
        }

        PlayerPrefs.Save();
        UpdateAchievementUI();
    }
}
