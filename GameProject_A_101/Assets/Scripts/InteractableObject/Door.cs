using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{

    [Header("문 설정")]
    public bool isOpen = false;
    public Vector3 openPosition;
    public float openSpeed = 2.0f;

    private Vector3 closedPosition;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();                           //기존 상속 받은 스타트를 함수를 한번 실행 시킨다
        objectName = "문";
        interactionText = "[E] 문 열기";
        interactionType = InteractionType.Building;
        

        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.right * 3f;                     //오른쪽으로 3미터 이동
    }

    protected override void AccessBuilding()
    {
         isOpen = !isOpen;
        if(isOpen )
        {
            interactionText = "[E] 문 닫기";
            StartCoroutine(MoveDoor(openPosition));
        }
        else
        {
            interactionText = "[E] 문 열기";
            StartCoroutine(MoveDoor(closedPosition));
        }
    }

    IEnumerator MoveDoor(Vector3 targerPosition)
    {
        while (Vector3.Distance(transform.position, targerPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targerPosition, openSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targerPosition;
    }
}
