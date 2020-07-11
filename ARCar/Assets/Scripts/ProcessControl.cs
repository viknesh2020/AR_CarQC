using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class ProcessControl : MonoBehaviour
{
    public GameObject infoImage;
    public TextMeshProUGUI instrText;
    public GameObject confirmButton;
    public GameObject tapIcon;
    public GameObject[] tyreInfo;
    public GameObject wipers;
    public GameObject[] lights;
    
    private TextMeshProUGUI infoText;
    private TapAndPlace tp;
    private ARPlaneManager planeManager;
    private Animator wiperAnimator;
    private bool hitSurface;
       
    public void Awake()
    {
        infoText = infoImage.GetComponentInChildren<TextMeshProUGUI>();
        tp = GameObject.Find("AR Session Origin").GetComponent<TapAndPlace>();
        planeManager = GameObject.Find("AR Session Origin").GetComponent<ARPlaneManager>();
        wiperAnimator = wipers.GetComponent<Animator>();
        hitSurface = false;

        if (hitSurface == false)
        {
            StartCoroutine(ScanForSurface());
        }

        foreach(GameObject tyres in tyreInfo)
        {
            tyres.SetActive(false);
        }

        foreach(GameObject light in lights)
        {
            light.SetActive(false);
        }
    }

    public void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.GetComponent<ARPlane>() != null && hitSurface == false)
            {
                hitSurface = true;
            }
        }
    }


    IEnumerator ScanForSurface()
    {
       
       confirmButton.SetActive(false);
       instrText.SetText("Instruction: Wait till the application scanning for the surface.");
       infoImage.SetActive(true);
       infoText.SetText("Scanning for surfaces...");
       tapIcon.SetActive(false);
       yield return new WaitUntil(() => hitSurface);
       StartCoroutine(TapToPlace());        
    }

    IEnumerator TapToPlace()
    {
        instrText.SetText("Instruction: Tap anywhere on the detected surface.");
        infoImage.SetActive(false);
        infoText.text = "";
        confirmButton.SetActive(true);
        tapIcon.SetActive(true);
        yield return new WaitUntil(() => tp.objectSpawned);
        StartCoroutine(CheckTyrePressure());
    }

    IEnumerator CheckTyrePressure()
    {
        tapIcon.SetActive(false);
        instrText.SetText("Instruction: Check all the tyres pressure");
        infoImage.SetActive(true);
        infoText.text = "The ideal pressure value must be between 33 to 42 PSI";
        tyreInfo[0].SetActive(true);
        yield return new WaitForSeconds(5);
        instrText.SetText("Instruction: if the pressure is ideal, press confirm to check next tyre");
        yield return new WaitUntil(() => tp.buttonClicked);
        tyreInfo[0].SetActive(false);
        tyreInfo[1].SetActive(true);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => tp.buttonClicked);
        tyreInfo[1].SetActive(false);
        tyreInfo[2].SetActive(true);
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => tp.buttonClicked);
        tyreInfo[2].SetActive(false);
        tyreInfo[3].SetActive(true);
        yield return new WaitForSeconds(1);
        instrText.SetText("Instruction: All tyres are in ideal pressure value");
        tyreInfo[3].SetActive(false);
        yield return new WaitForSeconds(3);
        instrText.SetText("Instruction: Press confirm to check the wipers");
        yield return new WaitUntil(() => tp.buttonClicked);
        StartCoroutine(CheckWipers());
    }

    IEnumerator CheckWipers()
    {
        infoImage.SetActive(false);
        infoText.text = "";
        instrText.SetText("Instruction: Check the wipers");
        infoImage.SetActive(true);
        infoText.text = "Wipers must be running smooth without clog";
        wiperAnimator.SetBool("wiperon", true);
        yield return new WaitForSeconds(3);
        instrText.SetText("Instruction: if the wipers work fine, press confirm");
        yield return new WaitUntil(() => tp.buttonClicked);
        wiperAnimator.SetBool("wiperon", false);
        StartCoroutine(CheckLights());
    }

    IEnumerator CheckLights()
    {
        infoImage.SetActive(false);
        infoText.text = "";
        instrText.SetText("Instruction: Check the headlights. Press confirm to turn on the lights.");
        infoImage.SetActive(true);
        infoText.text = "Check all the four lamps in the headlights";
        yield return new WaitUntil(() => tp.buttonClicked);
        foreach (GameObject light in lights)
        {
            light.SetActive(true);
        }
        yield return new WaitForSeconds(5);
        instrText.SetText("Instruction: If lights are working good, press confirm");
        yield return new WaitUntil(() => tp.buttonClicked);
        foreach (GameObject light in lights)
        {
            light.SetActive(false);
        }
        instrText.SetText("Instruction: Quality Check process completed!");
        infoText.text = "The car is ready for the next process";
        yield return new WaitForSeconds(5);
        infoText.text = "Thank you. You can exit the application now";
    }
}
