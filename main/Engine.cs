using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//class is sealed because shpip's control system is designed only for this type of engines
public sealed class Engine : MonoBehaviour{

    [Tooltip("force engine applies to the ship")]
	public float strenght;
	float pwr;
	ParticleSystem ps;
	Rigidbody2D r;
	Transform t;
    NetworkIdentity myNetworkIdentity;

	void Start() {
        myNetworkIdentity = GetComponentInParent<NetworkIdentity>();

        pwr = 0;
		ps = GetComponent<ParticleSystem> ();
		r = GetComponentInParent<Rigidbody2D> ();
		t = GetComponent<Transform> ();
	}
    
	public void SetPower(float power){
		pwr = strenght * power;
	}


	void Update() {
		
		if(pwr<=0) {
			ps.Stop();
		}else{
			ps.Play();
		}
	
	}

	void FixedUpdate(){
        if (!myNetworkIdentity.isServer) { return; }
		if (pwr > 0) {
			r.AddForceAtPosition (t.up * pwr * Time.fixedDeltaTime, t.position, ForceMode2D.Impulse);
		}

	}

}
