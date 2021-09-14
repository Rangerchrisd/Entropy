using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionSellectCharacterSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public CharacterSheet characterSheet;
    public Image imageSlot;
    public TMP_Text characterNameText;
    //public static event Action<CharacterSheet> sellectCharacter;
    private CanvasGroup canvasGroup;
    public Vector3 orignial;

    public int siblingIndex;
    public Transform parentTransform;
    public RectTransform rectTransform;
    public GameObject SuperCanvas;
    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        orignial = rectTransform.localPosition;
        parentTransform = transform.parent;
        siblingIndex = transform.GetSiblingIndex();
        canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        loadSlot(characterSheet);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(characterSheet)
            imageSlot.transform.position = Input.mousePosition;
    }
    public void loadSlot(CharacterSheet characterSheetInput)
    {
        characterSheet = characterSheetInput;

        if (characterSheet)
        {
            imageSlot.sprite = characterSheet.myImage;
            characterNameText.text = characterSheet.unitName;
            characterNameText.enabled = true;
        }
        else
        {
            imageSlot.sprite = null;
            characterNameText.enabled = false;

        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out MissionSellectCharacterSlot otherCharacterSlot))
        {
            CharacterSheet tempCharacter = characterSheet;
            loadSlot(otherCharacterSlot.characterSheet);
            otherCharacterSlot.loadSlot(tempCharacter);
        }
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
        rectTransform.localPosition = orignial;
        transform.parent.SetSiblingIndex(siblingIndex);

    }
}
