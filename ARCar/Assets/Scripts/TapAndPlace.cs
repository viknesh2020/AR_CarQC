using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;

public class TapAndPlace : MonoBehaviour
{
    private ARSessionOrigin sessionOrigin;
    private ARRaycastManager castManager;
    private List<ARRaycastHit> hits;

    public GameObject model;
    public GameObject canvas;
    [HideInInspector]
    public bool objectSpawned;
    [HideInInspector]
    public bool buttonClicked;

    void Start()

    {
        sessionOrigin = GetComponent<ARSessionOrigin>();
        castManager = GetComponent<ARRaycastManager>();
        hits = new List<ARRaycastHit>();

        objectSpawned = false;
        buttonClicked = false;
        model.SetActive(false);
        canvas.SetActive(false);
    }

    void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        

        if (castManager.Raycast(touch.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon) && !IsPointerOverUIObject(touch.position))
        {
            Pose pose = hits[0].pose;


            if (!model.activeInHierarchy)
            {
                model.SetActive(true);
                model.transform.position = pose.position;
                model.transform.rotation = pose.rotation;
                objectSpawned = true;
                canvas.SetActive(true);
            }
            else
            {
                model.transform.position = pose.position;
            }
        }
    }

    bool IsPointerOverUIObject(Vector2 pos)
    {
        if (EventSystem.current == null)
            return false;
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(pos.x, pos.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void ConfirmClicked()
    {
        buttonClicked = true;
        Invoke("ClickFalse", 0.5f);
    }

    public void ClickFalse()
    {
        buttonClicked = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
