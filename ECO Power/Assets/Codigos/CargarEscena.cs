using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CargarEscena : MonoBehaviour
{
    public UnityEvent cargando;

    public void Cargar(string nombre)
    {
        StartCoroutine(CargarAsincrona(nombre));
    }

    public void Salir()
    {
        Application.Quit();
    }
    IEnumerator CargarAsincrona(string nombre)
    {
        cargando.Invoke();

        yield return new WaitForSeconds(1.5f);

        AsyncOperation operacion = SceneManager.LoadSceneAsync(nombre);

        while (!operacion.isDone)
        {
            yield return null;
        }
    }
}
