using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadState : ActionBaseState
{
    public override void EnterState(ActionStateManager actions)
    {
        actions.rHandAim.weight = 0;//else the animation dosent work properly
        actions.lHandIK.weight = 0;//else the animation dosent work properly
        actions.anim.SetTrigger("Reload");
    }

    public override void UpdateState(ActionStateManager actions)
    {

    }
}
