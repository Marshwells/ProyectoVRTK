using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ModoDisparo2
{
    SemiAuto,
    FullAuto
}


public class LogicaArma2 : MonoBehaviour
{



    protected Animator animator;
    protected AudioSource audioSource;
    public bool tiempoNoDisparo = false;
    public bool puedeDisparar = false;
    public bool recargando = false;

    [Header(" Referencia de Objetos")]
    //public ParticleSystem FuegoArma;
    public Camera camaraPrincipal;
    public Transform PuntoDeDisparo;

    [Header(" Referencia de Sonido")]
    public AudioClip SonidoDiparo;
    public AudioClip SonidoCartuchoEntra;
    public AudioClip SonidoCartuchoSale;
    public AudioClip SonidoVacio;
    public AudioClip SonidoDesenfundar;


    [Header(" Atributos del arma")]
    public ModoDisparo2 modoDisparo2 = ModoDisparo2.FullAuto;
    public float daño = 20f;
    public float ritmodisparo = 0.3f;
    public int balasrestantes;
    public int balascartucho;
    public int tamañocartucho = 30;
    public int maximodebalas = 100;
    // variables del zoom del arma al aplicar el Fire2 (Click derecho)
    public bool estaADS = false;
    public Vector3 disCadera;
    public Vector3 ADS;
    public float tiempoApuntar;
    public float zoom;
    public float normal;



    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();


        balascartucho = tamañocartucho;
        balasrestantes = maximodebalas;


        Invoke("HabilitarArma",0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(modoDisparo2 == ModoDisparo2.FullAuto && Input.GetButton("Fire1"))
        {
            RevisarDisparo();

        }else if (modoDisparo2 == ModoDisparo2.SemiAuto && Input.GetButtonDown("Fire1"))
        {
            RevisarDisparo();
        }

        if(Input.GetButtonDown("Reload"))
        {
            RevisarRecarga();
        }

        if (Input.GetMouseButton(1))
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, ADS, tiempoApuntar * Time.deltaTime);
            estaADS = true;
            camaraPrincipal.fieldOfView = Mathf.Lerp(camaraPrincipal.fieldOfView, zoom, tiempoApuntar * Time.deltaTime);

        }

        if (Input.GetMouseButtonUp(1))
        {
            estaADS = false;
        }

        if(estaADS == false)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, disCadera, tiempoApuntar * Time.deltaTime);
            
            camaraPrincipal.fieldOfView = Mathf.Lerp(camaraPrincipal.fieldOfView, normal, tiempoApuntar * Time.deltaTime);
        }

    }

    void HabilitarArma()
    {
        puedeDisparar = true;
    }

    void RevisarDisparo()
    {
        if (!puedeDisparar) return;
        if (tiempoNoDisparo) return;
        if (recargando) return;
        if (balascartucho > 0)
        {
            Disparar();
        }
        else
        {
            SinaBalas();
        }
    }

    // 0  -0.91   -0.04 
    void Disparar()
    {
        audioSource.PlayOneShot(SonidoDiparo);
        tiempoNoDisparo = true;
       // FuegoArma.Stop();
        //FuegoArma.Play();
        ReproducirAnimacionDisparo();
        balascartucho--;
        StartCoroutine(ReiniciarTiempoNoDisparo());
        DisparoDirecto();
    }


    void DisparoDirecto()
    {
        RaycastHit hit;
        if(Physics.Raycast(PuntoDeDisparo.position, PuntoDeDisparo.forward, out hit))
        {
            if (hit.transform.CompareTag("Enemigo"))
            {
                Vida vida = hit.transform.GetComponent<Vida>();
                if(vida == null)
                {
                    throw new System.Exception("No Tiene vida el ");
                }
                else
                {
                    vida.RecibirDaño(daño);
                }
            }
        }
    }

    public virtual void ReproducirAnimacionDisparo()
    {
        if(gameObject.name == "UMP45")
        {
            if(balascartucho > 1)
            {
                animator.CrossFadeInFixedTime("Fire",0.1f);
            }
            else
            {
                animator.CrossFadeInFixedTime("Fire",0.1f);
            }
        }
        else
        {
            animator.CrossFadeInFixedTime("Fire",0.1f);
        }
    }

    void SinaBalas()
    {
        audioSource.PlayOneShot(SonidoVacio);
        tiempoNoDisparo = true;
        StartCoroutine(ReiniciarTiempoNoDisparo());
    }

    IEnumerator ReiniciarTiempoNoDisparo()
    {
        yield return new WaitForSeconds(ritmodisparo);
        tiempoNoDisparo = false;
    }

    void RevisarRecarga()
    {
        if(balasrestantes > 0 && balascartucho < tamañocartucho)
        {
            Recargar();
        }
    }


     void Recargar()
    {
        if (recargando) return;
        recargando = true;
        animator.CrossFadeInFixedTime("Reload", 0.1f);

        
    }

    void RecargarMuniciones()
    {
        int balasParaRecargar = tamañocartucho - balascartucho;
        int restarBalas = (balasrestantes >= balasParaRecargar) ? balasParaRecargar : balasrestantes;

        balasrestantes -= restarBalas;
        balascartucho += balasParaRecargar;
    }

    public void DesenfundarON()
    {
        audioSource.PlayOneShot(SonidoDesenfundar);
    }

    public void CartuchoEntraON()
    {
        audioSource.PlayOneShot(SonidoCartuchoEntra);
        RecargarMuniciones();
    }

    public void CartuchoSaleON()
    {
        audioSource.PlayOneShot(SonidoCartuchoSale);
        
    }

    public void VacioON()
    {
        audioSource.PlayOneShot(SonidoVacio);
        Invoke("ReiniciarRecargar",0.1f);
    }

    void ReiniciarRecargar()
    {
        recargando = false;

    }
}
