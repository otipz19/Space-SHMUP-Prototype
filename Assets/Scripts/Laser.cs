using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LayerMask layerMask;
    private LineRenderer lineRenderer;
    private WeaponDefinition laserDef;

    private void Start()
    {
        laserDef = Main.GetWeaponDefinition(WeaponType.laser);
        layerMask = LayerMask.GetMask("Enemy");

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineRenderer.endWidth = laserDef.laserWidth;
        lineRenderer.enabled = false;
    }

    public void LaserCast(Vector3 origin)
    {
        RaycastHit hit;

        origin.z = 0;
        Vector3 originRight = origin; originRight.x += laserDef.laserWidth / 2;
        Vector3 originLeft = origin; originLeft.x -= laserDef.laserWidth / 2;   

        if ( Physics.Raycast(originRight, Vector3.up, out hit, laserDef.laserDistance, layerMask) ||
             Physics.Raycast(originLeft, Vector3.up, out hit, laserDef.laserDistance, layerMask) )
        {
            DrawLine(origin, hit.distance, laserDef.projectileColor);
            Enemy hitEnemy = hit.collider.transform.root.GetComponent<Enemy>();
            hitEnemy.GetDamage(laserDef.damagePerSecond * Time.deltaTime, hit.collider);
        }
        else
        { 
            DrawLine(origin, laserDef.laserDistance, laserDef.weaponColor);
        }
    }

    public void LaserUnCast()
    {
        lineRenderer.enabled = false;   
    }

    private void DrawLine(Vector3 origin, float distance, Color color)
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.startColor = lineRenderer.endColor = color;
        Vector3 hitPoint = origin; hitPoint.y += distance;
        lineRenderer.SetPositions(new Vector3[] { origin, hitPoint });
    }
}