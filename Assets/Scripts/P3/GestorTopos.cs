using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GestorTopos : MonoBehaviour
{
    public int puntuacion = 0;
    public int puntosAcierto = 10;
    public int puntosFallo = 10;
    public Topo[] listaTopos;

    private float timer = 0;
    public float tiempoEntreTopos = 1f;
    private float tiempoVidaTopos;

    // Referencia estática para el singleton
    static public GestorTopos instance = null;

    public AudioClip musica;
    public AudioClip clickJugador;

    public TextMeshPro textoPuntos;

    // Si la partuda ha acaado es true
    public bool gameOver = false;

    // Topos que aparecen en cada ronda
    public int toposPorRonda = 10;
    private int toposRestantes;
    public TextMeshPro contadorText;
    public TextMeshPro finDeRonda;
    // Topos que han aparecido en esta ronda hasta ahora
    private int toposSpawneados = 0;

    // El awakee se llama antes del start
    private void Awake()
    {
        // Asignamos la instancia de la clase para el singleton
        // Si no hay instancia asignada, asigno este
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        // si hay instancia asignada, destruyo este objeto porque solo debe haber un singleton en escena
        else Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // listaTopos = FindObjectsOfType<Topo>(); // Busca en toa la escena
        listaTopos = GetComponentsInChildren<Topo>(); // Busca entre los hijos de GestorTopos
        tiempoVidaTopos = listaTopos[0].tAparece + listaTopos[0].tDesaparece + listaTopos[0].tEspera;
        SoundManager.instance.PlayMusic(musica);
        toposRestantes = toposPorRonda;
        finDeRonda.gameObject.SetActive(false);
        ActualizaMarcador();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOver)
        {
            // Activa un topo cada tiempoEntreTopos segundos
            timer += Time.deltaTime;
            if (timer >= tiempoEntreTopos)
            {
                ActivaTopoAleatorio();
                timer = 0;
                toposSpawneados++;
                toposRestantes--;
                CompruebaFinDeRonda();
            }
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            PulsacionJugador();
        }
    }

    public void ActivaTopoAleatorio()
    {
        bool topoEncontrado =false;
        int intentos = 0;
        while (!topoEncontrado && intentos <100) // Mientras topoEncontrado es false (no se ha encontrado)
        {
            // Obtengo una posición de la lista de topos
            // Entre 0 y el tamaño de la lista
            int random = Random.Range(0, listaTopos.Length - 1);

            // compruebo que el topo esté inactivo antes de activarlo
            if (listaTopos[random].estado == EstadoTopo.INACTIVO)
            {
                listaTopos[random].ActivateMole();  
                topoEncontrado = true;
            }
            intentos++;
        }
    }

    // Reiniciar valores para una partida nueva
    private void ResetPartida()
    {
        toposRestantes = toposPorRonda;
        finDeRonda.gameObject.SetActive(false);
        puntuacion = 0;
        ActualizaMarcador();
        timer = 0;
        gameOver = false;
        toposSpawneados = 0;
        // Reiniciar los topos
        // For normal
        /*
        for (int i = 0; i < listaTopos.Length; i++)
        {
            listaTopos[i].ResetMole();
        }*/

        // For each - siempre recorre la lista completa
        foreach (Topo topo in listaTopos)
        {
            topo.ResetMole();
        }
    }

    private void CompruebaFinDeRonda()
    {
        /* Rondas hasta tener x puntuación
        if(puntuacion > 100)
        {
            gameOver = true;
        }*/

        // Ronda hasta aparecer x topos
        if(toposSpawneados >= toposPorRonda)
        {
            gameOver = true;
            Invoke("MensajeFinal", tiempoVidaTopos);
        }
    }
    public void PulsacionJugador()
    {
        Debug.Log("Pulsación");
        // Me guardo un ray entre la camara  la posición del ratón
        // Lo hago con el método ScreenPointToRay de la cámara
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Debug.Log("PINTANDO RAYO");
        // Debug.DrawRay(ray.origin, ray.direction, Color.red, 4);
        RaycastHit infoRayo;

        if(Physics.Raycast(ray, out infoRayo))
        {
            if(infoRayo.transform.tag == "BotonReset")
            {
                //He chocado con el boton de reset
                ResetPartida();
            }

            else // si no es un botón, compruebo si es un topo
            {
                Topo auxComptopo = infoRayo.transform.gameObject.GetComponentInParent<Topo>();

                if (auxComptopo != null) 
                {
                    if (auxComptopo.estado == EstadoTopo.APARECE || auxComptopo.estado == EstadoTopo.ESPERA)
                    {
                        auxComptopo.OnMoleHit();
                    }
                }
            }

        }
    }

    public void GanaPuntos()
    {
        puntuacion += puntosAcierto;
        ActualizaMarcador();
    }

    public void PierdePuntos()
    {
        puntuacion -= puntosFallo;
        if(puntuacion < 0 ) puntuacion = 0;
        ActualizaMarcador();
    }

    public void ActualizaMarcador()
    {
        textoPuntos.text = puntuacion.ToString();
        contadorText.text = toposRestantes.ToString();
    }

    public void MensajeFinal()
    {
        finDeRonda.gameObject.SetActive(true);
    }
}
