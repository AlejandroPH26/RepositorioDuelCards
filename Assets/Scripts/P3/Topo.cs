using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EstadoTopo {INACTIVO, APARECE, ESPERA, DESAPARECE}

public class Topo : MonoBehaviour
{
    public EstadoTopo estado = EstadoTopo.INACTIVO;

    // Tiempos quepasa en cada estado antes de cambiar
    public float tAparece = 1f;
    public float tEspera = 1.5f;
    public float tDesaparece = 0.5f;

    public Vector3 escalaMin = Vector3.zero;
    public Vector3 escalaMax = new Vector3(1, 1, 1);

    public float zEnterrado = -1.237f;
    public float zDescubierto = -2.31f;

    public float timer = 0f;
    private GestorTopos gt;

    public AudioClip sfxGolpe;
    public AudioClip sfxFallo;

    public GameObject spriteAcierto;
    public GameObject spriteFallo;

    public ParticleSystem particulasGolpe;


    // Start is called before the first frame update
    void Start()
    {
        // Guardamos referencia al gestor para abreviar el código
        // La guardamos en una variable de la clase desde el instance (singleton)
        gt = GestorTopos.instance;

        spriteAcierto.SetActive(false);
        spriteFallo.SetActive(false);
    }

    private void OnEnable() // Se llama cada vez que se activa este componente
    {
        ResetMole(); // Resetea posición y avlores del topo
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (estado)
        {
            // No hace nada, está desactivado
            case EstadoTopo.INACTIVO:
                break;
            // Animación de aparecer y pasa a esperar
            case EstadoTopo.APARECE:
                Apareciendo();
                break;
            // Espera un tiempo o hasta quele golpean y cambia a desaparecer
            case EstadoTopo.ESPERA:
                Esperando();
                break;
            // Animación de desaparecer y pasa a inactivo
            case EstadoTopo.DESAPARECE:
                Desapareciendo();
                break;
        }
    }

    void Apareciendo()
    {
        timer += Time.deltaTime;
        // ANIMACIÓN DE APARECER CON ESCALA
        // Interpolamos la escaa entre min y max durante tAparece tiempo
        transform.localScale = Vector3.Lerp(escalaMin, escalaMax, timer / tAparece);

        // ANIMACION APARECER CON MOVIMIENTO
        Vector3 auxPos = transform.localPosition; // Me guardo la posición actual
        // Calcula la posicion en el eje Z (altura) y se la asigno al vector
        auxPos.z = Mathf.Lerp(zEnterrado, zDescubierto, timer / tAparece);
        transform.localPosition = auxPos; // Asigno la posición a objeto

        // Comprobamos que ha pasado tAparecer
        if (timer >= tAparece)
        {
            // Pasar a esperar
            estado = EstadoTopo.ESPERA;
            // Reinicio el timer
            timer = 0f;
        }
    }

    void Esperando()
    {
        timer += Time.deltaTime;

        // Comprobamos que ha pasado tEsperar

        if(timer >= tEspera)
        {
            SoundManager.instance.PlaySFX(sfxFallo);
            gt.PierdePuntos();
            // Pasar a desaparecer
            estado = EstadoTopo.DESAPARECE;
            // Reinicio del timer
            timer = 0f;
            spriteFallo.SetActive(true);
        }
    }

    void Desapareciendo()
    {
        timer += Time.deltaTime;

        // ANIMACIÓN DE DESAPARECER
        // Interpolamos la escaa entre max y mindurante tDesaparece tiempo
        transform.localScale = Vector3.Lerp(escalaMax, escalaMin, timer / tDesaparece);

        // ANIMACION DESAPARECER CON MOVIMIENTO
        Vector3 auxPos = transform.localPosition; // Me guardo la posición actual
        // Calcula la posicion en el eje Z (altura) y se la asigno al vector
        auxPos.z = Mathf.Lerp(zDescubierto, zEnterrado, timer / tDesaparece);
        transform.localPosition = auxPos; // Asigno la posición a objeto

        // ANIMACION APARECER CON MOVIMIENTO

        // Comprobamos que ha pasado tDesaparece

        if (timer >= tDesaparece)
        {
            // Pasar a inactivo
            estado = EstadoTopo.INACTIVO;
            // Reinicio del timer
            timer = 0f;
            spriteAcierto.SetActive(false);
            spriteFallo.SetActive(false);
        }
    }

    public void ResetMole()
    {
        estado = EstadoTopo.INACTIVO;
        transform.localScale = escalaMin;
        Vector3 aux = transform.localPosition;
        aux.z = zEnterrado;
        transform.localPosition = aux;
        timer = 0f;

        spriteAcierto.SetActive(false);
        spriteFallo.SetActive(false);

        particulasGolpe.Clear();
    }

    public void ActivateMole()
    {
        estado = EstadoTopo.APARECE;
        transform.localScale = escalaMin;
        Vector3 aux = transform.localPosition;
        aux.z = zEnterrado;
        transform.localPosition = aux;
        timer = 0f;
    }

    public void OnMoleHit()
    {
        estado = EstadoTopo.DESAPARECE;
        transform.localScale = escalaMax;
        Vector3 aux = transform.localPosition;
        aux.z = zDescubierto;
        transform.localPosition = aux;
        timer = 0f;

        spriteAcierto.SetActive(true);

        SoundManager.instance.PlaySFX(sfxGolpe);
        gt.GanaPuntos();

        // particulasGolpe.Play();
        particulasGolpe.Emit(5);
    }
}
