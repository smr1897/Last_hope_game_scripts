using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missions : MonoBehaviour
{
    public bool mission1 = false;
    public bool mission2 = false;
    public bool mission3 = false;
    public bool mission4 = false;

    public void Update()
    {
        if(!mission1 && !mission2 && !mission3 && !mission4) 
        {
            Debug.Log("locate the camp");
        }
        if(mission1 && !mission2 && !mission3 && !mission4)
        {
            Debug.Log("Locate the intel");
        }
        if(mission1 && mission2 && !mission3 && !mission4)
        {
            Debug.Log("Plant the bomb");
        }
        if(mission1 && mission2 && mission3 && !mission4)
        {
            Debug.Log("Exit the camp");
        }
    }

}
