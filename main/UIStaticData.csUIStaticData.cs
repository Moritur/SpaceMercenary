using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIStaticData {

    /* 
     * This enum contains list of all subclasses of Module class.
     * It's used in ship customization menu for holding data about
     * installed modules and is also necesarry for saving ship's
     * configuration to json file.
     * 
     * Whenever new module that can be used by player is added 
     * this enum must be updated manually. Modules not intended
     * for player's use don't have to be listed here.
     */
    public enum moduleList
    {
        None,
        HullReinforcement,
        AutoRepair
    }

    /* 
     * This enum contains names of all ships avalible to player.
     * It's used by UIShip class to load saved data about ship's
     * customization.
     */
    public enum shipNames
    {
        NameNotSet,
        EscapePod
    }

}
