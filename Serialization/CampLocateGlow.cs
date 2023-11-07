using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampLocateGlow : MonoBehaviour
{
    public Missions mission;
    public GameObject player;

    public void OnTriggerEnter(Collider other)
    {
        if (mission.mission2 == false && mission.mission3 == false && mission.mission4 == false)
        {
            mission.mission1 = true;
        }
    }

}
