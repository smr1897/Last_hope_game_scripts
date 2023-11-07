using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBomb : MonoBehaviour
{
    public Missions mission;
    public GameObject player;

    public void OnTriggerEnter(Collider other)
    {
        if (mission.mission1 == true && mission.mission2 == true && mission.mission4 == false)
        {
            mission.mission3 = true;
        }
    }
}
