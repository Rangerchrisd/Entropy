using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionSellectMissionSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Mission mission;
    public Image imageSlot;
    public TMP_Text missionNameText;
    public bool activeMission;
    public Vector3 orignial;
    //public static event Action<Mission> sellectMission;
    private CanvasGroup canvasGroup;

    public int siblingIndex;
    public Transform parentTransform;
    public RectTransform rectTransform;
    public GameObject SuperCanvas;
    public GameObject originalParent;
    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        orignial = rectTransform.localPosition;
        parentTransform = transform.parent;
        siblingIndex = transform.GetSiblingIndex();
        canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        loadSlot(mission);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (mission)
            imageSlot.transform.position = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (mission)
        {
            //sellectMission?.Invoke(mission);
            this.transform.position = orignial;
            //if()
        }
    }
    public void loadSlot(Mission missionInput) {
        mission = missionInput;
        if (mission)
        {
            imageSlot.sprite = mission.missionPreview;
            missionNameText.text = mission.missionName;
            missionNameText.enabled = true;
        }
        else {
            if (activeMission)
            {

                //imageSlot.sprite = null;

            }
            else {
                //imageSlot.enabled = false;
            }
            imageSlot.sprite = null;
            missionNameText.enabled = false;

        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out MissionSellectMissionSlot otherMissionSlot)){
            Mission tempMission = mission;
            loadSlot(otherMissionSlot.mission);
            otherMissionSlot.loadSlot(tempMission);
        }
       // throw new NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        this.gameObject.transform.SetParent(SuperCanvas.transform);
        this.gameObject.transform.SetAsLastSibling();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        this.gameObject.transform.SetParent(parentTransform);
        transform.parent.SetSiblingIndex(siblingIndex);
        rectTransform.localPosition = orignial;
    }
}
