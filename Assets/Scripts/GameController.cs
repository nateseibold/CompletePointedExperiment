using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public Camera cameraStart;
    public Camera cameraGame;
    public Camera cameraBlack;
    public Camera cameraEnd;

    public GameObject input;
    public GameObject canvas;
    public GameObject endCanvas;

    public GameObject starterBall;
    public GameObject ball;
    private GameObject ballInstance;
    public GameObject startingPoint;
    public GameObject endingPoint;
    public GameObject pointer;

    private float groundY = 0.35F;
    private int totalExperiments = 10;

    public float maxHalfDistance = 5;

    public bool startBall = false;

    public int experimentNumber = 0;

    public string participantID;
    public float travelTime;

    // Start is called before the first frame update
    void Start()
    {
        cameraStart.enabled = true;
        cameraGame.enabled = false;
        cameraBlack.enabled = false;
        cameraEnd.enabled = false;
        endCanvas.SetActive(false);

        pointer.SetActive(false);

        InputField inputField = input.GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        //Sets the new positions of the starting point and ending point, if the trial can be started
        if(startBall && experimentNumber <= totalExperiments)
        {
            experimentNumber++;
            startBall = false;
            pointer.SetActive(false);
            randomTrial();
        }

        else if(experimentNumber > totalExperiments)
        {
            endCanvas.SetActive(true);
            cameraGame.enabled = false;
            cameraEnd.enabled = true;
        }
    }

    //Moves the ball and sets the randomized time of the journey
    private void randomTrial()
    {
        ball.GetComponent<MoveBallArc>().sunrise = startingPoint.transform;
        ball.GetComponent<MoveBallArc>().sunset = endingPoint.transform;
        travelTime = UnityEngine.Random.Range(0.5f, 5.0f);
        ball.GetComponent<MoveBallArc>().journeyTime = travelTime;
        ball.GetComponent<MoveBallArc>().canStart = true;
        ballInstance = (GameObject)Instantiate(ball, startingPoint.transform.position, ball.transform.rotation);
    }

    //Obtains the ID entered for the participant
    public void enterID(string id)
    {
       participantID = id;
    }

    //Switches the Camera from UI to the VR camera to start the experiment
    public void switchCamera()
    {
        cameraStart.enabled = false;
        cameraGame.enabled = true;
        cameraBlack.enabled = false;
        canvas.SetActive(false);
        StartCoroutine(stallBall());
    }

    //Stalls the Ball so that it appears on the screen
    private IEnumerator stallBall()
    {
        float randomZ = UnityEngine.Random.Range(3.0F, 10.0F);
        startingPoint.transform.position = new Vector3(UnityEngine.Random.Range(maxHalfDistance * -1, 0.0F), groundY, randomZ);
        endingPoint.transform.position = new Vector3(UnityEngine.Random.Range(0.5F, maxHalfDistance), groundY, randomZ);
        GameObject ball = (GameObject) Instantiate(starterBall, startingPoint.transform.position, starterBall.transform.rotation);

        yield return new WaitForSeconds(1f);
        Destroy(ball);
        startBall = true;
    }

    //Called when the ball lands at the ending spot
    public void changeScreenEvent()
    {
        StartCoroutine(fadeAway());
    }

    //Goes to the black screen after waiting 3 seconds
    private IEnumerator fadeAway()
    {
        yield return new WaitForSeconds(3);
        cameraGame.enabled = false;
        cameraBlack.enabled = true;
        Destroy(ballInstance);
        StartCoroutine(fadeBack());
    }

    //Goes back to the main scene, but with the ball gone
    private IEnumerator fadeBack()
    {
        yield return new WaitForSeconds(3);
        cameraGame.enabled = true;
        cameraBlack.enabled = false;
        pointer.SetActive(true);
    }
}
 