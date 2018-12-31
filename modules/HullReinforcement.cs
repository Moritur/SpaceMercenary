using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullReinforcement : Module {

    public override void InitServer()
    {
        ShipStats shipStats = GetComponentInParent<ShipStats>();

        shipStats.maxHp += 50;
        shipStats.hp += 50;
    }

    public override void InitPlayer() { }
    public override void Visuals() { }
}