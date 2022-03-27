using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;
using UnityEngine.UI;

public class VRInputModule : BaseInputModule
{
    // Start is called before the first frame update
    public Camera Camera;
    public SteamVR_Input_Sources TargetSource;
    public SteamVR_ActionSet ActionSet;
    public SteamVR_Action_Boolean Click;
    private GameObject currentObject = null;
    private PointerEventData data = null;

    protected override void Awake()
    {
        base.Awake();
        ActionSet.Activate();
        data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        // reset data
        data.Reset();
        data.position = new Vector2(Camera.pixelWidth /2 , Camera.pixelHeight/2);

        //raycast
        eventSystem.RaycastAll(data, m_RaycastResultCache);
        data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);

        currentObject = data.pointerCurrentRaycast.gameObject;

        //clear
        m_RaycastResultCache.Clear();

        //Hoever
        HandlePointerExitAndEnter(data, currentObject);

        //Press
        if(Click.GetStateDown(TargetSource)) {
            ProcessPress(data);
        }

        //release
        if(Click.GetStateUp(TargetSource)) {
            ProcessRelease(data);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PointerEventData GetData() {
        return data;
    }

    private void ProcessPress(PointerEventData _data) {
        //set raycase
        _data.pointerPressRaycast = _data.pointerCurrentRaycast;


        //click for obnject hit, get the down handler, call
       GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(currentObject, _data, ExecuteEvents.pointerDownHandler);


        //if we dont have down handler, try and get click handler
        if (newPointerPress == null) {
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);
        }

        // setdata
        _data.pressPosition = _data.position;
        _data.pointerPress = newPointerPress;
        _data.rawPointerPress = currentObject;
    }

    private void ProcessRelease(PointerEventData _data) {
        // execute pointer up
        ExecuteEvents.Execute(_data.pointerPress, _data, ExecuteEvents.pointerUpHandler);


        // check for click handler
        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);

        //check if actual
        if(_data.pointerPress == pointerUpHandler) {
            ExecuteEvents.Execute(_data.pointerPress, _data, ExecuteEvents.pointerClickHandler);
        }

        // clear slected game object
        eventSystem.SetSelectedGameObject(null);

        // reset data
        _data.pressPosition = Vector2.zero;
        _data.pointerPress = null;
        data.rawPointerPress = null;

    }
}
