using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LogicaEnemigo : MonoBehaviour
{
    private GameObject target;
    private NavMeshAgent agente;
    private Vida vida;
    private Animator animator;
    private Collider collider;
    private Vida vidaJugador;
    private LogicaJugador logicaJugador;
    public bool Vida0 = false;
    public bool estaAtacando = false;
    public float speed = 1.0f;
    public float angularSpeed = 120;
    public float daño = 25;
   // public Transform other;
    public bool mirando;

    public bool sumarpuntos = false;
    public GameObject puntajePantalla;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Caracter");
        vidaJugador = target.GetComponent<Vida>();
        if (vidaJugador == null)
        {
            throw new System.Exception(" EL objeto Caracter no tiene componente vida");
        }

        logicaJugador = target.GetComponent<LogicaJugador>();

        if (logicaJugador == null)
        {
            throw new System.Exception(" EL objeto Caracter no tiene componente Logica JUgador");
        }

        agente = GetComponent<NavMeshAgent>();
        vida = GetComponent<Vida>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();


    }
        // Update is called once per frame
        void Update()
    {
        RevisarVida();
        Perseguir();
        RevisarAtaque();
        EstaDeFrenteJugador();

    }

    void EstaDeFrenteJugador()
    {
        Vector3 adelante = transform.forward;
        Vector3 targetJugador = (GameObject.Find("Caracter").transform.position - transform.position).normalized;

        if (Vector3.Dot(adelante, targetJugador) < 0.6f)
        {
            mirando = false;
        }
        else
        {
            mirando = true;
        }
    }


    void RevisarVida()
    {
        if (Vida0) return;
        if(vida.valor <= 0)
        {
            sumarpuntos = true;
            if (sumarpuntos)
            {
                puntajePantalla.GetComponent<Puntaje>().valor += 20;
                sumarpuntos = false;
            }
            Vida0 = true;
            agente.isStopped = true;
            collider.enabled = false;
            animator.CrossFadeInFixedTime("Vida0",0.1f);
            Destroy(gameObject, 1.5f);

        }
    }

    void Perseguir()
    {
        if (Vida0) return;
        if (logicaJugador.Vida0) return;
        agente.destination = target.transform.position;
    }


    void RevisarAtaque()
    {
        if (Vida0) return;
        if (estaAtacando) return;
        if (logicaJugador.Vida0) return;
        float distanciaDelBlanco = Vector3.Distance(target.transform.position, transform.position);

        if(distanciaDelBlanco <= 2.0 && mirando)
        {
            Atacar();
        }
    }

    void Atacar()
    {
        vidaJugador.RecibirDaño(daño);
        agente.speed = 0;
        agente.angularSpeed = 0;
        estaAtacando = true;
        animator.SetTrigger("DebeAtacar");
        Invoke("ReiniciarAtaque", 1.5f);
    }

    void ReiniciarAtaque()
    {
        estaAtacando = false;
        agente.speed = speed;

        agente.angularSpeed = angularSpeed;
    }
}

