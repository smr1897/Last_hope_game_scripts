using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocateIntel : MonoBehaviour
{
    public Missions mission;
    public GameObject player;

    public void OnTriggerEnter(Collider other)
    {
        if (mission.mission1 == true && mission.mission3 == false && mission.mission4 == false)
        {
            mission.mission2 = true;
        }
    }
}
