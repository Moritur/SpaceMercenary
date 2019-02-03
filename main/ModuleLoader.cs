using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(ShipControl))]
public class ModuleLoader : NetworkBehaviour {

    ShipControl shipControl;
    UIStaticData.shipNames shipName;
    UIStaticData.moduleList[] choosenModules;   //list of modules player choose
    GameObject[] moduleObjects;                 //gameObjects to wich Module components should be added
    bool modulesLoaded = false;                 //true if modules were loaded in this instance

    void Awake()
    {
        shipControl = GetComponent<ShipControl>();
        shipName = shipControl.shipName;
        moduleObjects = shipControl.moduleObjects;
    }

    void Start () {
        if (!isLocalPlayer) { return; }

        //use instance of UIShip to load module data it saved when player customized ship
        UIShip uis = gameObject.AddComponent<UIShip>();
        uis.shipName = shipName;    //shipName defines which file will be used to load data
        uis.UpdatePath();   //update path to use correct shipName
        uis.Load();         //load saved data
        CmdLoadModules(uis.moduleList);
        Destroy(uis);
    }

    /*
     * TargetLoadModules can't be called directly in this case, as it needs correct choosenModules
     * and CallTargetLoadModules is called from instance of this class attached to the ship
     * of player who just joined the game on instance attached to the ship of other player in new
     * players instance of game.
     * 
     * tl;dr
     * 
     * In some cases TargetLoadModules can't be called directly and has to be called by CallTargetLoadModules
     */

    public void CallTargetLoadModules(NetworkConnection target)
    {
        TargetLoadModules(target, choosenModules);
    }

    [TargetRpc]
    void TargetLoadModules(NetworkConnection target, UIStaticData.moduleList[] mdls)
    {
        if (modulesLoaded) { return; }
        choosenModules = mdls;
        LoadModules();
    }

    [ClientRpc]
    void RpcLoadModules(UIStaticData.moduleList[] mdls)
    {
        if (modulesLoaded) { return; }
        choosenModules = mdls;
        LoadModules();
    }

    void SpawnModule(UIStaticData.moduleList component, int id)
    {
        moduleObjects[id].AddComponent(ModuleListToComponent(component));
    }

    //Load modules player choose for his ship
    [Command]
    void CmdLoadModules(UIStaticData.moduleList[] modulesToLoad)
    {
        choosenModules = modulesToLoad;
        LoadModules();      //load this ship's modules on server
        RpcLoadModules(choosenModules);     //load this ship's modules for all players who wre currently in game
        GetComponentInParent<ShipStats>().SetStatsToMax();     //make sure modules altering max hp and armor take effect
        GameObject[] ships = GameObject.FindGameObjectsWithTag("Player");

        //this loop loads modules of all ships that are already connected when player who called this command joins in his game instance
            foreach (GameObject g in ships)
            {
                if (g != gameObject)
                {
                    g.GetComponent<ModuleLoader>()?.CallTargetLoadModules(connectionToClient);
                }
            }
    }

    void LoadModules()
    {
        int i = 0;
        while (i < choosenModules.Length)
        {
            SpawnModule(choosenModules[i], i);
            i++;
        }
        if (choosenModules.Length < moduleObjects.Length)
        {
            SpawnModule(UIStaticData.moduleList.None, i);
            i++;
        }
        modulesLoaded = true;
        shipControl.StartModules();
    }

    System.Type ModuleListToComponent(UIStaticData.moduleList moduleList)
    {

        //no breaks in this switch as in all cases method returns
        switch (moduleList)
        {
            case UIStaticData.moduleList.None:
                return typeof(EmptyModule);

            case UIStaticData.moduleList.HullReinforcement:
                return typeof(HullReinforcement);

            case UIStaticData.moduleList.AutoRepair:
                return typeof(AutoRepair);

            case UIStaticData.moduleList.ImprovedArmor:
                return typeof(ImprovedArmor);

            case UIStaticData.moduleList.NavigationSystems:
                return typeof(NavigationSystems);

            case UIStaticData.moduleList.TemporaryArmor:
                return typeof(TemporaryArmor);

            default:
                Debug.LogWarning("No Module class assigned for this module, check ModuleListToComponent method in ModuleLoader class");
                return typeof(EmptyModule);
        }
    }

}
