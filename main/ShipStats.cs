using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public sealed class ShipStats : NetworkBehaviour {

    [SyncVar]
    public float maxHp;
    [SyncVar][HideInInspector]
    public float hp;
    [SyncVar]
	public float maxArmor;
    [SyncVar][HideInInspector]
    public float armor;
	Text txthp;             //UI text used to display current hp in hex
	RectTransform barHp;    //bar used to display current hp

    void Awake()
    {
        //hp and armor values must be set in Awake, otherwise modules altering them may not work properly
        hp = maxHp;
        armor = maxArmor;
    }

    void Start () {
        if (!isLocalPlayer) { return; }

        txthp = GameObject.Find("hp").GetComponent<Text>();
        barHp = GameObject.Find("barHp").GetComponent<RectTransform>();
	}
	
	void Update () {
        if (!isLocalPlayer) { return; }

        txthp.text = ((int)hp).ToString("X");
		barHp.localScale = new Vector3 ((hp/239), 1f, 1f);

#if (DEVELOPMENT_BUILD || UNITY_EDITOR)
        //damage your ship by pressing L; for testing purposes
        if (Input.GetKeyDown (KeyCode.L)) {
			CmdSelfHarm (15);
		}
#endif
    }
    [Server]
	void Hit (float damage) {
		if (damage < armor) { return;}
		hp -= (damage-armor);
	}

    [Command]
    void CmdSelfHarm(float damage) {
        hp -= damage;
    }

    [Server]
    public void Repair(float repairedHp)
    {
        /* 
         * There are four options:
         * 1.   ship has already full hp
         * 2.   after repair ship will have full hp
         * 3.   after repair ship will have more hp, but still not full
         * 4.   ship for some reason now has more hp than max and it's value shouln't be changed
         *      as this state was probably caused intentionally by some module
         */
        if (hp >= maxHp) { return; }            //case 1. and 4.

        if(hp+repairedHp>=maxHp & hp < maxHp)   //case 2.
        {
            hp = maxHp;
        }
        else if(hp+repairedHp<maxHp)            //case 3.
        {
            hp += repairedHp;
        }
    }
}
