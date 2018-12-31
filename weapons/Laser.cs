using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : Weapon {

    RaycastHit2D hit;
    LineRenderer laserRay;
    Vector3[] laserPositions = new Vector3[2];

    protected override void CStart()
    {
        laserRay = GetComponent<LineRenderer>();
        laserRay.enabled = false;
    }

    protected override void CShoot()
    {
       hit = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y),transform.up);
        if (hit)
        {
            hit.transform.SendMessage("Hit", damage, SendMessageOptions.DontRequireReceiver);
        }
    }

    protected override void CReload() { }

    public override void ShootVisuals()
    {
        hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), transform.up);
        StartCoroutine("EnableRay");
        laserPositions[0] = transform.position;
        if (hit)
        {
            laserPositions[1] = hit.point;
        }
        else
        {
            laserPositions[1] = transform.position + transform.up * 20;
        }
        laserRay.SetPositions(laserPositions);
    }

    IEnumerator EnableRay()
    {
        laserRay.enabled = true;
        //laser ray has to be visible for at least 0.4s, but shouldn't last longer than 1s
        yield return new WaitForSeconds(Mathf.Clamp(shootingSpeed,0.4f,1f)); 
        laserRay.enabled = false;       
    }
}
