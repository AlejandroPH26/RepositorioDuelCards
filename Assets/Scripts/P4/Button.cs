using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public AudioClip sfxPress;
    public ParticleSystem particles;

    public bool isActivated;

    // Start is called before the first frame update
    void Start()
    {
        isActivated = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            PulsacionJugador();
        }
    }

    public void SoundOn()
    {
        SoundManager_P4.instance.PlaySFX(sfxPress);

        /*
        timer += Time.deltaTime;
        Vector3 auxPos = transform.localPosition;
        auxPos.x = Mathf.Lerp(posicionInicial, posicionFinal, timer / tiempoTransicion);
        transform.localPosition = auxPos; 
        */
    }

    public void ParticlesOnOff()
    {
        if (isActivated == false)
        {
            particles.Play();
            isActivated = true;
        }
        else
        {
            particles.Stop();
            isActivated = false;
        }

    }

    public void PulsacionJugador()
    {
        //Debug.Log("Pulsación");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit infoRayo;

        if (Physics.Raycast(ray, out infoRayo))
        {
            if (infoRayo.transform.tag == "BotonPush")
            {
                SoundOn();
            }

            if (infoRayo.transform.tag == "BotonParticles")
            {
                ParticlesOnOff();
            }
        }
    }
}
