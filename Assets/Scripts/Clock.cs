using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Clock : MonoBehaviour {

    public Transform hoursTransform, minutesTransform, secondsTransform;

    private void Awake() {
        hoursTransform.Rotate(0, DateTime.Now.Hour * 30, 0);
        minutesTransform.Rotate(0, DateTime.Now.Minute * 6, 0);
        secondsTransform.Rotate(0, DateTime.Now.Second * 6, 0);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        hoursTransform.Rotate(0, Time.deltaTime/(60*60), 0);
        minutesTransform.Rotate(0, Time.deltaTime / (60), 0);
        secondsTransform.Rotate(0, Time.deltaTime, 0);
    }
}
