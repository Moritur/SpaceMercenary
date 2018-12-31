using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//module spawned when player didn't choose any
public sealed class EmptyModule : Module {

    public override void InitServer() { }
    public override void InitPlayer() { }
    public override void Visuals() { }
}
