using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HullReinforcement : Module {

    const int hpAdded = 50;

    public override void InitServer()
    {
        ShipStats shipStats = GetComponentInParent<ShipStats>();

        shipStats.maxHp += hpAdded;
        shipStats.hp += hpAdded;
    }

    public override void InitPlayer() { }
    public override void Visuals() { }
}