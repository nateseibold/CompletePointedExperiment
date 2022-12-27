using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class positionalData : MonoBehaviour
{
    public bool printBool = false;
    public GameObject playerHead;
    public GameObject pointer;
    //public GameObject scripts;
    //public GameObject experimentInfo;
    public string dataTracked;
    public string m_Path;
    public string m_Path2;
    public string application_Path;

    string nearPermaCue;

    // Start is called before the first frame update
    void Start()
    {
        application_Path = Application.dataPath;
        application_Path = application_Path + "/"+"headData.txt";
        m_Path = application_Path;

        string header = "Time,Subject ID,Trial Number,Travel Time,Actual Start Point,Participant Start Point,Actual End Point,Participant End Point,Distance betweeen Actual and Participant Start,Distance between Actual and Participant End";
        StreamWriter writer8 = new StreamWriter(m_Path, true);
        writer8.WriteLine(header);
        writer8.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrintLine()
    {
        //Data
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        double cur_time = (double)(System.DateTime.UtcNow - epochStart).TotalMilliseconds;
        cur_time = cur_time * 1000;

        string id = GetComponent<GameController>().participantID;
        float time = GetComponent<GameController>().travelTime;
        Vector3 startActual = GetComponent<GameController>().startingPoint.transform.position;
        Vector3 startPlayer = pointer.GetComponent<Pointer>().startClick;
        Vector3 endActual = GetComponent<GameController>().endingPoint.transform.position;
        Vector3 endPlayer = pointer.GetComponent<Pointer>().endClick;

        dataTracked = cur_time.ToString("n0").Replace("," , "") + "," + id + "," + time + "," + startActual +  "," + startPlayer + "," + endActual +  "," + endPlayer + "," + Vector3.Distance(startActual, startPlayer) + "," + Vector3.Distance(endActual, endPlayer) + "\n";
        StreamWriter writer8 = new StreamWriter(m_Path, true);
        writer8.WriteLine(dataTracked);
        writer8.Close();
    }

    public void printAndClose()
    {
        printBool = true;
    }
}
