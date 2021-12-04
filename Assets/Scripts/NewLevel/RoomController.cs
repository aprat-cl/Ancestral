using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    // Start is called before the first frame update
    public RoomInstance room;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player entered:" + Convert.ToString(room.Spec));
            PlayerData.CurrentRoom = room;
        }
    }
}
