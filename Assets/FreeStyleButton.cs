using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FreeStyleButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerClickHandler
{
    
    // Start is called before the first frame update
    public void OnPointerEnter(PointerEventData eventData) {
       //Debug.Log("<color=red>Event:</color> Completed mouse highlight.");
    }

    public void OnSelect(BaseEventData eventData) {
        // Debug.Log("<color=red>Event:</color> COmpleted selection.");
    }

    public void OnPointerClick(PointerEventData eventData) {
        
    }

}
