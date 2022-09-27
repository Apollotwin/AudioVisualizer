using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncStrobe : AudioSyncer
{
    public override void OnUpdate()
    {
        base.OnUpdate();
        
        //Check if we are in a beat state
        
        if(isBeat) return;
        
        // if not in a beat state reset values
    }

    public override void OnBeat()
    {
        base.OnBeat();
        
        
    }
    
}
