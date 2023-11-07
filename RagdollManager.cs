using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    Rigidbody[] rbs;

    // Start is called before the first frame update
    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>(); //This is used to get all the rigidbody componenets in the children of the game object on which this script is attached
        foreach (Rigidbody rb in rbs) rb.isKinematic = true;
        //when we add rigidbody to a game object that means we simply added physics to it
        //But when we set each of the rigidbody components to is kinematic we remove those physics properties from the game object
        //So it won't be affected by physics forces any more
    }

    public void TriggerRagdoll()
    {
        foreach (Rigidbody rb in rbs) rb.isKinematic = false ;
    }
}
