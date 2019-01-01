using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody2D),typeof(ModuleLoader))]
public abstract class ShipControl : NetworkBehaviour {

	abstract public void OnStart();         //use this in derived class instead of Start 
	abstract public void OnUpdate();        //use this in derived class instead of Update
	abstract public void OnFixedUpdate();   //called only for local player

    [Tooltip("Unique name of this ship model (list can be edited in UIStaticData script)")]
    public UIStaticData.shipNames shipName; //used to identify ships model
    [Tooltip("List if ship's engines")]
    public GameObject[] engines;
    [Tooltip("List of ship's weapons")]
    public Weapon[] weapons;
    [Tooltip("List of ship's modules")]
    public GameObject[] moduleObjects;      //gameObjects representing modules

    Module[] modules;                       //Ship's modules. This array's values are set in StartModules method
    bool modulesReady = false;              //true after modules have been spawned and initialized

    protected float[] enginesPowers;        //current power (0-1) of each engine; engines update their power settings from this array in FixedUpdate
    protected NetworkIdentity myNetworkIdentity;
 
    protected void Movement(float[] engpow) //called from derived class after it determines which engines should be at which power levels
    {
        bool equal = true;
        int j = 0;
       foreach(float f in engpow)
        {
            if (f != enginesPowers[j]) { equal = false; }
            j++;
        }
        if (equal) { return; }
        if (engpow.Length != enginesPowers.Length) { return; }
        for(int i = 0; i < engpow.Length; i++)
        {
            engpow[i] = Mathf.Clamp01(engpow[i]);
        }
        CmdDoMovement(engpow);
    }

    [Command]
    public void CmdDoMovement(float[] pps) {
   
        enginesPowers = pps;
        RpcUpdateEngines(pps);
    }

    
    [ClientRpc]
    void RpcUpdateEngines(float[] sps)      //every player needs updated engine states for visual effects
    {
        enginesPowers = sps;
        int j = 0;
        foreach (float f in enginesPowers)
        {
            engines[j].SendMessage("SetPower", f);
            j++;
        }
    }

    [Command]
    void CmdInitializeModulesServer()
    {
        foreach(Module m in modules)
        {
            m.InitServer();
        }
    }

    void InitializeModulesPlayer()
    {
        foreach (Module m in modules)
        {
            m.InitPlayer();
        }
    }

    [Command]
    void CmdUseModule(int id)     //calls Module.Use() on server
    {
        if (id >= modules.Length) { return; }   //aoid IndexOutOfRange exception

        if (modules[id].Use())  //if module is disabled or damaged don't create visual effects
        {
            RpcModuleVisuals(id);
        }
    }

    [ClientRpc]
    void RpcModuleVisuals(int id)     //calls Module.Visuals() for every player
    {
        modules[id].Visuals();
    }

    void ModuleControls()       //Controls for activation of modules. Called only for LocalPlayer
    {
        if (!modulesReady) { return; }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CmdUseModule(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CmdUseModule(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CmdUseModule(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CmdUseModule(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CmdUseModule(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            CmdUseModule(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            CmdUseModule(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            CmdUseModule(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CmdUseModule(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CmdUseModule(9);
        }
    }

    public void StartModules()       //called by ModuleLoader after it loads all modules
    {
        modules = new Module[moduleObjects.Length];
        int i = 0;
        foreach (GameObject g in moduleObjects)
        {
            modules[i] = g.GetComponent<Module>();
            i++;
        }

        if (isLocalPlayer)
        {
            CmdInitializeModulesServer();
            InitializeModulesPlayer();
        }
        modulesReady = true;
    }

   

	void Start () {
        if (!isLocalPlayer) { return; }

		enginesPowers = new float[engines.Length];
       
		OnStart ();
	}

	void FixedUpdate() {

        if (isLocalPlayer)
        {
            OnFixedUpdate();
            int j = 0;
            foreach (float f in enginesPowers)
            {
                engines[j].SendMessage("SetPower", f);
                j++;
            }
        }
        
     

	}

    void Update()
    {
        if (isLocalPlayer)
        {
            ModuleControls();
        }
        OnUpdate();
    }

    [Command]
    protected void CmdShootAll()
    {
        for(short i=0;i<weapons.Length;i++)
        {
            if (weapons[i].CanShoot()) {
                RpcShootVisuals(i);
                weapons[i].Shoot();
            }
        }
    }

    [ClientRpc]
    protected void RpcShootVisuals(short id)
    {
        weapons[id].ShootVisuals();
    }

}
