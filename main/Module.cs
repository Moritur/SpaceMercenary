using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Module : MonoBehaviour{

    [HideInInspector]
    public bool working { get; protected set; } = true;     //set to false to disable module

    public abstract void InitServer();  //called on server when ship is spawned
    public abstract void InitPlayer();  //called on ship owner's instance when ship is spawned
    public abstract void Visuals();     //Called by ClientRPC when player uses module. Purpose of this method is to make visual effects visible for everyone
    protected virtual bool CUse() { return false; } //called on server when player uses module, returns false if module can't be used

    public bool Use()
    {
        if (working) {
            if (CUse())
            {
                return true;
            }
            else
            {
                //later some GUI message will be added here
                print("Module not responding");
                return false;
            }
        }
        else
        {
            //later some GUI message will be added here
            print("Module not responding");
            return false;
        }
    }

}
