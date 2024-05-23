using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ARButton : MonoBehaviour
{
    public UnityEvent action; // Variable que permite asignar una funcion a llamar de un objeto en escena

    private void OnMouseDown() // Se llama al hacer click sobre este objeto
    {
        action.Invoke(); // Llama a la funcion de action
    }
}
