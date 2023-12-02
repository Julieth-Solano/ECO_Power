using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ManejadorRecoleccion : MonoBehaviour
{
    public Transform contenedorElementos;
    public int cantidadElementos;

    public UnityEvent finalizoRecoleccion;
    // Start is called before the first frame update
    void Start()
    {
        cantidadElementos = contenedorElementos.childCount;
    }

    public void ContarElementos()
    {
        cantidadElementos--;
        if (cantidadElementos == 0)
            finalizoRecoleccion.Invoke();
    }
}
