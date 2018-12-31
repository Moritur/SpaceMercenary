using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEscapePod : ShipControl {

    float[] powers;

	override public void OnStart () {
        powers = new float[enginesPowers.Length];
	}
	
	override public void OnUpdate () {
        if (isLocalPlayer & Input.GetKeyDown(KeyCode.LeftShift))
        {
            CmdShootAll();  //shoot all weapons
        }
    }

    /*
     * IMPORTANT NOTE
     * Code below requires uisng key combinations W+S+A and W+S+D to move sideways.
     * Some keyboards may not be able to detect those combinations and this is
     * a hardware problem.
     */
    
	override public void OnFixedUpdate() {
        for (int i = 0; i < powers.Length; i++)
        {
            powers[i] = 0f;
        }
        //W     move forward
        if (Input.GetKey (KeyCode.W) && !Input.GetKey (KeyCode.S) && !Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.D)) {
			powers [0] = 1f;
			powers [1] = 1f;
			powers [2] = 1f;
		}else
		//S     move backward
		if (!Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.S) && !Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.D)) {
			powers [3] = 1f;
			powers [4] = 1f;
		}else
		//A     rotate left
		if (!Input.GetKey (KeyCode.W) && !Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.D)) {
			powers [1] = 0.7f;
			powers [4] = 0.7f;
		}else
		//D     rotate right
		if (!Input.GetKey (KeyCode.W) && !Input.GetKey (KeyCode.S) && !Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.D)) {
			powers [2] = 0.7f;
			powers [3] = 0.7f;
		}else
		//WA    move forward and rotate left
		if (Input.GetKey (KeyCode.W) && !Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.D)) {
			powers [0] = 1f;
			powers [1] = 0.5f;
			powers [4] = 0.5f;
		}else
		//WD    move forward and rotate right
		if (Input.GetKey (KeyCode.W) && !Input.GetKey (KeyCode.S) && !Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.D)) {
			powers [0] = 1f;
			powers [2] = 0.5f;
			powers [3] = 0.5f;
		}else
		//SA    move backward and rotate left
		if (!Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.D)) {
			powers [1] = 1f;
			powers [3] = 1f;
			powers [4] = 1f;
		}else
		//SD    move backward and rotate right
		if (!Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.S) && !Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.D)) {
			powers [2] = 1f;
			powers [3] = 1f;
			powers [4] = 1f;
		}else
		//WSA   move left
		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.D)) {
			powers [2] = 1f;
			powers [4] = 0.96f;
		}else
		//WSD   move right
		if (Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.S) && !Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.D)) {
			powers [1] = 1f;
			powers [3] = 0.96f;
		}
        Movement(powers);
        
	}
    
    
}
