using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorEnemigos : MonoBehaviour

{
    public GameObject zombiePrefab;
    public Transform[] puntosDeGeneracion;
    public float tiempoDeGeneracion = 5f;


    // Start is called before the first frame update
    void Start()
    {
        puntosDeGeneracion = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            puntosDeGeneracion[i] = transform.GetChild(i);
        }

        StartCoroutine(AparecerEnemigo());
    }

    IEnumerator AparecerEnemigo()
    {
        while (true)
        {
            for (int i = 0; i < puntosDeGeneracion.Length; i++)
            {
                Transform puntodeGeneracion = puntosDeGeneracion[i];
                Instantiate(zombiePrefab, puntodeGeneracion.position, puntodeGeneracion.rotation);

            }
            yield return new WaitForSeconds(tiempoDeGeneracion);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
