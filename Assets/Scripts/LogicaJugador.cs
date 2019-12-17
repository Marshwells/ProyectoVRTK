 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicaJugador : MonoBehaviour
{
    public Vida vida;
    public bool Vida0 = false;
    public Puntaje puntaje;
    [SerializeField] private Animator animadorPerder;
    // Start is called before the first frame update
    void Start()
    {
        vida = GetComponent<Vida>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vida.valor <= 0)
        {
            Application.LoadLevel(2);
        }
    }

   
}
