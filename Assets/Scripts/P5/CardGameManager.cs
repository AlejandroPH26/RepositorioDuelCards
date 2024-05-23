using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardGameManager : MonoBehaviour
{
    // Lista estática de cartas en la escena (targets)
    [SerializeField] private ARCard[] cartasEnEscena;
    // Lista dinámica de cartas detectadas por la cámara
    [SerializeField] private List<ARCard> cartasTrackeadas;
    
    public int nCartasTrackeadas = 0; // Nº de cartas trackeadas
    private int nCartasDerrotadas = 0; // Nº de cartas derrotadas

    // Elementos de interfaz
    public GameObject botonResetPartida; // Botón de reset de la partida
    public GameObject botonResetCombate; // Botón de reset combate
    public GameObject botonCombate;
    public TextMeshProUGUI ganaJ1Text;
    public TextMeshProUGUI ganaJ2Text;
    public Slider sliderJ1;
    public Slider sliderJ2;
    public int vidaMaxima = 150;

    // Posiciones para cálculo jugadores
    public Transform posJ1;
    public Transform posJ2;

    // Cartas de cada jugador
    private ARCard cartaJ1;
    private ARCard cartaJ2;

    // Estado del juego
    private bool juegoTerminado = false;


    // Start is called before the first frame update
    void Start()
    {
        cartasEnEscena = FindObjectsOfType<ARCard>();
        sliderJ1.maxValue = sliderJ1.value = vidaMaxima;
        sliderJ2.maxValue = sliderJ2.value = vidaMaxima;
        botonResetPartida.SetActive(false); // Inicializar botón de reset partida como inactivo
        botonResetCombate.SetActive(false); // Inicializar botón de reset combate como inactivo
        ganaJ1Text.gameObject.SetActive(false);
        ganaJ2Text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!juegoTerminado)
        {
            ActualizaListaCartas();
            CompruebaBotonCombate();
            CompruebaPropietarioCartas();
        }
    }

    public void ResetGame()
    {
        // Reseteamos todas las cartas
        foreach(ARCard card in cartasEnEscena)
        {
            card.ResetCarta();
        }
        // Reset cartas destruidas
        nCartasDerrotadas = 0;
        // Reset a la vida de cada jugador
        sliderJ1.value = vidaMaxima;
        sliderJ2.value = vidaMaxima;
        // Reset estado del juego
        juegoTerminado = false;
        botonCombate.SetActive(false);
        botonResetCombate.SetActive(false);
        botonResetPartida.SetActive(false);
        ganaJ1Text.gameObject.SetActive(false);
        ganaJ2Text.gameObject.SetActive(false);
    }

    public void ResetCombate()
    {
        // Reseteamos las posiciones de las cartas
        foreach (ARCard card in cartasEnEscena)
        {
            card.ResetCarta();
        }
        // Reset cartas destruidas
        nCartasDerrotadas = 0;
        // No resetear la vida de los jugadores
        // Reset estado del juego
        juegoTerminado = false;
        botonCombate.SetActive(false);
        botonResetCombate.SetActive(false); // Desactivar botón de reset combate después de usarlo
        botonResetPartida.SetActive(false);
    }

    public void Combate()
    {
        // TO DO: Distinguir de qué jugador es cada carta

        // Obtenemos su estadística (ATK si esta en ataque, DEF si esta en defensa)
        int statJ1 = cartaJ1.GetCardStat();
        int statJ2 = cartaJ2.GetCardStat();

        // Comparamos las 2 estadísticas
        // Si gana un jugador, el otro pierde la diferencia de estadística
        // La carta que pierde se destruye
        // Si hay enpate se destruyen las dos
        if(statJ1 > statJ2 ) // Gana J1
        {
            // Destruimos carta J2
            cartaJ2.PonEstado(CardPose.DESTROYED);
            //nCartasDerrotadas ++;
            // Restar vida a J2
            sliderJ2.value -= statJ1 - statJ2;
        }
        else if (statJ2 > statJ1 ) // Gana J2
        {
            // Destruimos carta J1
            cartaJ1.PonEstado(CardPose.DESTROYED);
            //nCartasDerrotadas++;
            // Restar vida a J1
            sliderJ1.value -= statJ2 - statJ1;
        }
        else // Empatan, se destruyen las dos cartas
        {
            cartaJ1.PonEstado(CardPose.DESTROYED);
            cartaJ2.PonEstado(CardPose.DESTROYED);
            //nCartasDerrotadas += 2;
            sliderJ1.value -= statJ2 - statJ1;
            sliderJ2.value -= statJ1 - statJ2;
        }
        SoundManagerP5.instance.PlaySFX(0);
        CheckGameOver();
        // botonResetCombate.SetActive(true); // Activar botón de reset combate después del combate
    }

    private void CompruebaPropietarioCartas()
    {
        if(nCartasTrackeadas >= 2)
        {
            Transform posC1 = cartasTrackeadas[0].transform; // posición carta nº1
            Transform posC2 = cartasTrackeadas[1].transform; // posición carta nº2

            // Calculamos las distancias entre cada jugador y las dos cartas
            float dist_J1_C1 = Vector3.Distance(posJ1.position, posC1.position);
            float dist_J1_C2 = Vector3.Distance(posJ1.position, posC2.position);
            float dist_J2_C1 = Vector3.Distance(posJ2.position, posC1.position);
            float dist_J2_C2 = Vector3.Distance(posJ2.position, posC2.position);

            if(dist_J1_C1 < dist_J1_C2) // Si C1 es la mas cercana a J1
            {
                cartaJ1 = cartasTrackeadas[0]; // C1 es de J1
                cartaJ2 = cartasTrackeadas[1]; // C2 es de J2
            }
            else
            {
                cartaJ1 = cartasTrackeadas[1]; // C2 es de J1
                cartaJ2 = cartasTrackeadas[0]; // C1 es de J2
            }

            // Asignamos propietario a las cartas (sprites distintivos)
            cartaJ1.AsignaPropietario(false);
            cartaJ2.AsignaPropietario(true);
        }
    }
    private void CompruebaBotonCombate()
    {
        // Si hay 2 o mas cartas trackeadas, comprobamos estados
        if(nCartasTrackeadas >= 2)
        {
            ARCard carta1 = cartasTrackeadas[0];
            ARCard carta2 = cartasTrackeadas[1];

            // Si las dos cartas estan en atk o def, activamos el boton combate

            if ((carta1.pose == CardPose.ATTACK || carta1.pose == CardPose.DEFENSE) &&
                (carta2.pose == CardPose.ATTACK || carta2.pose == CardPose.DEFENSE))
            {
                botonCombate.SetActive(true);
            }
            else if(sliderJ1.value == 0 || sliderJ2.value == 0)
            {
                botonCombate.SetActive(false);
            }
            else botonCombate.SetActive(false);
        }
        // Si no, lo desactivamos
        else
        {
            botonCombate.SetActive(false);
        }

        // botonCombate.SetActive(nCartasTrackeadas >= 2); // VERSIÓN RESUMIDA
    }

    private void ActualizaListaCartas()
    {
        // Vaciar la lista de cartas
        cartasTrackeadas.Clear();
        nCartasTrackeadas = 0;

        // Recorrerla lista de cartas en escena
        foreach(ARCard carta in cartasEnEscena)
        {
            // Detectamos las cartas trackeadas
            if(carta.GetStatus() != Vuforia.Status.NO_POSE)
            {                                             
                cartasTrackeadas.Add(carta);            // Añadimos la carta a a lista de cartas trackeadas
                nCartasTrackeadas++;                    //Llevamos la cuenta de lascartas trackeadas
            }
        }
    }

    private void CheckGameOver()
    {
        // Si la vida de un jugador llega a 0, pierde
        if(sliderJ1.value <= 0) // Pierde J1
        {
            nCartasDerrotadas++;
            // Gana J2
            Debug.Log("Gana J2");
            SoundManagerP5.instance.PlaySFX(1);
            ganaJ2Text.gameObject.SetActive(true);
            botonResetCombate.SetActive(false);
            juegoTerminado = true;
        }
        else if(sliderJ2.value <= 0) // Pierde J2
        {
            nCartasDerrotadas++;
            // Gana J1
            Debug.Log("Gana J1");
            SoundManagerP5.instance.PlaySFX(1);
            ganaJ1Text.gameObject.SetActive(true);
            botonResetCombate.SetActive(false);
            juegoTerminado = true;
        }

        // Si no quedan cartas para hacer un combate, se acaba la partida
        if (nCartasDerrotadas >= cartasEnEscena.Length - 1)
        {
            // Gana el que tenga más vida
            if (sliderJ1.value > sliderJ2.value)
            {
                Debug.Log("Gana J1");
                ganaJ1Text.gameObject.SetActive(true);
                botonResetCombate.SetActive(false);
            }
            else if (sliderJ2.value > sliderJ1.value)
            {
                Debug.Log("Gana J2");
                ganaJ2Text.gameObject.SetActive(true);
                botonResetCombate.SetActive(false);
            }
            else
            {
                Debug.Log("Empate");
            }
            juegoTerminado = true;
        }

        if (juegoTerminado)
        {
            // Desactivar botón de combate para evitar más interacciones
            botonCombate.SetActive(false);
            botonResetCombate.SetActive(false);
            // Activar el botón de reset de partida
            botonResetPartida.SetActive(true); // Activar el botón de reset partida
        }
        else if (!juegoTerminado)
        {
            botonResetCombate.SetActive(true);
        }
    }
}
