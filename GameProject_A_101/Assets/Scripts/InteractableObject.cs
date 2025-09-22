using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    [Header("상호작용 정보")]
    public string objectName = "아이템";
    public string interactionText = "[E] 상호작용";
    public InteractionType interactionType = InteractionType.Item;              //타입은 우선 아이템

    [Header("하이라이트 설정")]
    public Color highlightColor = Color.yellow;
    public float highlightIntensity = 1.5f;

    public Renderer objectRenderer;
    private Color originalColor;
    private bool isHighlighted = false;

    public enum InteractionType
    { 
        Item,                   //아이템
        Machine,                //기계 (레버 버튼 등)
        Building,               //건물 (문 상자 등)
        NPC,                    //NPC
        Collectible
    }

    // Start is called before the first frame update
    

    protected virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if(objectRenderer != null )
        {
            originalColor = objectRenderer.material.color;
        }

        gameObject.layer = 8;
    }

    public virtual void OnPlalyerEnter()
    {
        Debug.Log($"{objectName}) 감지됨");
        HighlightObject();
    }

    public virtual void OnPlayerExit()
    {
        Debug.Log($"[{objectName}]에서 벗어남");
        RemoveHighlight();
    }

    public virtual void Interact()
    {
        //상호작용 타입에 따른 기본 동작
        switch (interactionType)
        {

            case InteractionType.Item:
                CollectItem();
                break;
            case InteractionType.Machine:
                OperateMachine();
                break;
            case InteractionType.Building:
                AccessBuilding();
                break;
            case InteractionType.Collectible:
                CollectItem();
                break; 
            case InteractionType.NPC:
                TalkToNPC();
                break;
        }
    }

    public string GetInteractionText()
    {
        return interactionText;
    }

    protected virtual void HighlightObject()    //protected는 c# 접근 권한 설정(해당 클래스 내부, 그리고 해당 클래스를 상속 받은 모든 파생 클래스에서 접근 가능)
    {                                           //virtual(가상 메서드) 함수의 속성을 가상으로 만들어서, 상속받는 파생 클래스가 해당 함수를 재정의 하도록 허용

        if( objectRenderer != null && !isHighlighted)
        {
            objectRenderer.material.color = highlightColor;
            objectRenderer.material.SetFloat("_Emission", highlightIntensity);
            isHighlighted = true;
        }
    }

    protected virtual void RemoveHighlight()
    {
        if(objectRenderer != null && !isHighlighted)
        {
            objectRenderer.material.color = originalColor;
            objectRenderer.material.SetFloat("_Emission", 0f);
            isHighlighted = false;
        }
    }

    protected virtual void CollectItem()
    {
        Debug.Log($"{objectName}을 획득했습니다!");
        Destroy(gameObject);
    }

    protected virtual void OperateMachine()
    {
        Debug.Log($"{objectRenderer}을 작동시켰습니다");
        if(objectRenderer != null)
        {
            objectRenderer.material.color = Color.green;
        }
    }

    protected virtual void AccessBuilding()
    {
        Debug.Log($"{objectName}을(를) 에 접근했습니다.");
        transform.Rotate(Vector3.up, 90f);
    }

    protected virtual void TalkToNPC()
    {
        Debug.Log($"{objectName}와의 대화를 시작합니다. ");
    }
}
