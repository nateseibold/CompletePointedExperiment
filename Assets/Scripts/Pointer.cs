using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Pointer : MonoBehaviour
{
    public float defaultLength = 5.0f;
    public GameObject dot;
    //public VRInputModule inputModule;

    private LineRenderer lineRenderer = null;

    public SteamVR_Input_Sources targetSource;
    public SteamVR_Action_Boolean clickAction;

    public Vector3 startClick;
    public Vector3 endClick;

    private int numClicks = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        float targetLength = defaultLength;

        RaycastHit hit = CreateRaycast(targetLength);

        Vector3 endPosition = transform.position + (transform.forward * targetLength);

        if(hit.collider != null)
        {
            endPosition = hit.point;
        }

        dot.transform.position = endPosition;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);

        if(clickAction.GetStateDown(targetSource) && numClicks == 0)
        {
            startClick = endPosition;
            numClicks++;
            //print subject number, trial number, "start point",math.distance(hit.point.x,hit.point.z), actual distance,  hit.point, actual start point, Time.time
        }

        if(clickAction.GetStateDown(targetSource) && numClicks == 1)
        {
            endClick = endPosition;
            numClicks = 0;
            //print subject number, trial number, "end point", math.distance(hit.point.x,hit.point.z), actual distance, hit.point, actual end point, Time.time
            StartCoroutine(newTrial());
        }
    }
    
    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, defaultLength);

        return hit;
    }

    private IEnumerator newTrial()
    {
        GameObject.Find("Camera").GetComponent<positionalData>().PrintLine();
        yield return new WaitForSeconds(1.5f);
        GameObject.Find("Camera").GetComponent<GameController>().startBall = true;
    }
}
