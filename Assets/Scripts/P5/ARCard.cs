using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vuforia;
public enum CardPose{DEFAULT, ATTACK, DEFENSE, DESTROYED };
public class ARCard : MonoBehaviour
{
    //Enum con los posibllels estados de la carta
    public CardPose pose = CardPose.DEFAULT; //Estado actual de esta carta

    private ImageTargetBehaviour target; //Componente del imagetarget

    public int ATK = 0;
    public int DEF = 0;

    public TextMeshPro textoATK, textoDEF; // Textos de carta
    public GameObject botonATK, botonDEF, modeloATK, spriteDEF, spriteDestruido, indicadorJ1, indicadorJ2; // Objetos de carta

    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<ImageTargetBehaviour>();
        ActualizaTextos();
        ActualizaSegunEstado();
    }

    // Reiniciar la carta al estado por defecto
    public void ResetCarta()
    {
        pose = CardPose.DEFAULT;        // Cambiamos estado
        ActualizaSegunEstado();         // Actualizamos objetos hijod
        indicadorJ1.SetActive(false);   // Apagamos indicadores de propietario
        indicadorJ2.SetActive(false);
    }

    public Status GetStatus()
    {
        return target.TargetStatus.Status;
    }

    public int GetCardStat()
    {
        if (pose == CardPose.ATTACK) // Ataque
        {
            return ATK;
        }
        else if (pose == CardPose.DEFENSE) // Defensa
        {
            return DEF;
        }
        else return -1; // Otro estado

        // VERSION CON OPERADOR TERNARIO
        // (Condicion entre parentesis) ? (codigo que se hace si es true) : (codigo que se hace si es false);
        // return  (pose == CardPose.ATTACK) ? ATK : DEF;
    }

    private void ActualizaTextos()
    {
        textoATK.text = "ATK: \n" + ATK.ToString();
        textoDEF.text = "DEF: \n" + DEF.ToString();     
    }

    public void PonPosicionAtaque()
    {
        pose = CardPose.ATTACK; // Cambia el estado de la carta a atacar
        ActualizaSegunEstado();
    }

    public void PonPosicionDefensa()
    {
        pose = CardPose.DEFENSE; // Cambia el estado de la carta a defender
        ActualizaSegunEstado();
    }
    public void PonEstadoDestruido()
    {
        pose = CardPose.DESTROYED;
        ActualizaSegunEstado();
    }

    public void PonEstado(CardPose estado)
    {
        pose = estado;
        ActualizaSegunEstado();
    }
    private void ActualizaSegunEstado()
    {
        switch(pose)
        {
            case CardPose.DEFAULT:
                botonATK.SetActive(true);
                botonDEF.SetActive(true);
                spriteDEF.SetActive(false);
                spriteDestruido.SetActive(false);
                modeloATK.SetActive(true);
                Debug.Log("Default");
                break;
            case CardPose.ATTACK:
                botonATK.SetActive(false);
                botonDEF.SetActive(false);
                spriteDEF.SetActive(false);
                spriteDestruido.SetActive(false);
                modeloATK.SetActive(true);
                Debug.Log("Attack");
                break;
            case CardPose.DEFENSE:
                botonATK.SetActive(false);
                botonDEF.SetActive(false);
                spriteDEF.SetActive(true);
                spriteDestruido.SetActive(false);
                modeloATK.SetActive(false);
                Debug.Log("Defense");
                break;
            case CardPose.DESTROYED:
                botonATK.SetActive(false);
                botonDEF.SetActive(false);
                spriteDEF.SetActive(false);
                spriteDestruido.SetActive(true);
                modeloATK.SetActive(false);
                Debug.Log("Destroyed");

                break;
            default:
                break;
        }
    }

    public void AsignaPropietario(bool j2)
    {
        indicadorJ1.SetActive(!j2);
        indicadorJ2.SetActive(j2);

        /*
        if (j2)
        {
            indicadorJ1.SetActive(false);
            indicadorJ2.SetActive(true);
        }
        else
        {
            indicadorJ1.SetActive(true);
            indicadorJ2.SetActive(false);
        }
        */
    }
}
