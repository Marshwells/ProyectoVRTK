using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantallaMunicion : MonoBehaviour
{

    public Text texto;
    public LogicaArma2 LogicaArma2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        texto.text = LogicaArma2.balascartucho + "/" + LogicaArma2.tamañocartucho + " \n" + LogicaArma2.balasrestantes;

    }
}
