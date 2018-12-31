using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour{

    public float damage;
    public float shootingSpeed;         //time between shoots in seconds
    public float reloadTime;
    public bool reloadable;             //can this weapon be reloaded?
    protected bool canShoot=true;       //can weapon already be fired after last shoot?
    protected bool isReloading=false;

    protected abstract void CShoot();   //method implementing actual shooting for specific weapon
    protected abstract void CReload();  //method implementing actual reloading
    protected abstract void CStart();   //use in derived class instead of Start
    public abstract void ShootVisuals();//Method creating visual effects. In multiplayer should be called by server as ClientRPC

    void Start()
    {
        canShoot = true;
        isReloading = false;
        CStart();
    }
   
    public void Shoot()
    {
        if (!CanShoot()) { return; }    //if can't shoot then just quit
        StartCoroutine("IEShoot");
    }

    /*
     * Dont confuse CanShoot method with canShoot variable. This method returns 
     * false when canShoot is false or when anything else prevents shooting, 
     * so it has to be overrinden in derived class if it adds anything that could 
     * prevent player from shooting.
     */
    public virtual bool CanShoot()
    {
        if (!canShoot | isReloading) { return false; }
        return true;
    }


    protected IEnumerator IEShoot()
    {
        CShoot();
        canShoot = false;               //block shooting until amount of time specified in shootingSpeed passes
        yield return new WaitForSeconds(shootingSpeed);
        canShoot = true;
    }

    public void Reload()
    {
        StartCoroutine("IEReload");
    }
	
	protected virtual IEnumerator IEReload () {
        if (!reloadable) { yield break; }   //if this weapon can't be reloaded just quit
        isReloading = true;
        CReload();
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
	}
}
