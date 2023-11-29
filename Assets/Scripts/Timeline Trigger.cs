using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables; //Add this at the start to gain functions for the timeline
public class TimelineTrigger : MonoBehaviour
{
   
    public PlayableDirector Entering_Timeline; //Name of the TimeLine slot for the script, visuble in Unity
                                     
    public GameObject ObjectToDeactivate1;


    private int TimeLinePlayValue;

    private void Start()
    {
        
    }

    // Start is called before the first frame update
    void OnTriggerEnter(Collider other) //Changed from Void Start (Doesn't start on play)
    
       

    {
        
        Entering_Timeline.Play();   //Reference the "Timeline_Event" in here with ".play();" at the end. 
            Debug.Log("Player stepped in trigger bounds. The Timeline has be started.");
        ObjectToDeactivate1.SetActive(false);
    }


   

     


    // Update is called once per frame
    void Update()
    {
        
    }
}
