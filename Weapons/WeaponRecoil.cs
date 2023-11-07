using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public Transform recoilFollowPos;
    [SerializeField] float kickBackAmount=-1;
    [SerializeField] float kickBackSpeed=10, returnSpeed=20;
    float currentRecoilPosition, finalRecoilPosition;

    // Update is called once per frame
    void Update()
    {
        currentRecoilPosition = Mathf.Lerp(currentRecoilPosition, 0, returnSpeed * Time.deltaTime);
        finalRecoilPosition = Mathf.Lerp(finalRecoilPosition, currentRecoilPosition, kickBackSpeed * Time.deltaTime);
        recoilFollowPos.localPosition = new Vector3(0, 0, finalRecoilPosition);
    }

    public void TriggerRecoil() => currentRecoilPosition += kickBackAmount;

    //Extra : This triggerRecoil function has only one line
    //We can also get rid of the brackets since it has only one line
    //Like this
    //public void triggerRecoil => currentRecoilPosition + kickBackAmount;

}
