using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ManejadorRecoleccion : MonoBehaviour
{
    public Transform contenedorElementos;
    public int cantidadElementos;
    public TextMeshProUGUI txtElementos;

    public UnityEvent finalizoRecoleccion;
    // Start is called before the first frame update
    void Start()
    {
        cantidadElementos = contenedorElementos.childCount;
        txtElementos.text = "" + cantidadElementos;
    }

    public void ContarElementos()
    {
        cantidadElementos--;
        txtElementos.text = "" + cantidadElementos;
        if (cantidadElementos == 0)
            finalizoRecoleccion.Invoke();
    }
}
