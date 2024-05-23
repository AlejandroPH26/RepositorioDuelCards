using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebRayCast : MonoBehaviour
{
    public bool laserContinuo = false;

    // Start is called before the first frame update
    void Start()
    {
        float resultadoCuenta;
        Division(100, 5, out resultadoCuenta);
    }

    // Update is called once per frame
    void Update()
    {
        if (laserContinuo)
        {
            
        }
        else if (Input.GetMouseButton(0))
        {
           

        }
        
    }

    public bool Division(float cociente, float divisor, out float resultado)
    {
        if (divisor == 0)
        {
            resultado = 0;
            return false;
        }
        resultado = cociente / divisor;

        return true;
    }



    public void LaserContinuo()
    {
        
        Ray ray = new Ray(transform.position, transform.forward);

        //Debug.Log("PINTANDO RAYO");
        Debug.DrawRay(ray.origin, ray.direction*10, Color.yellow, 0.5f);

        RaycastHit infoRayo;

        if (Physics.Raycast(ray, out infoRayo))
        {
            Debug.Log("He chocado con" + infoRayo.transform.gameObject.name);
            Debug.Log("Estaba a una distancia de" + infoRayo.distance);
            Debug.Log("He chocado en la posición" + infoRayo.point);
            Destroy(infoRayo.transform.gameObject);
        }
    }
}
