using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovimientoPersonaje : MonoBehaviour
{
 
    public float velocidad = 6;
    public float tiempoRotacion = 0.1f;
 
    public float fuerzaSalto = 1.0f;
    public float gravedad = 9.81f;

    public int vidas = 3;
    public UnityEvent murio;
    public List<GameObject> listaCorazones = new List<GameObject>();

    private float velocidadGiro;
    private float fuerzaGravedad = 0;

    private Vector3 posicionIncial;
    private Quaternion rotacionIncial;

    private bool caer=false;
    private CharacterController controladorCuerpo;
    private Animator animaciones;
    void Start()
    {
        posicionIncial = transform.position;
        rotacionIncial = transform.rotation;
        controladorCuerpo = GetComponent<CharacterController>();
        animaciones = GetComponent<Animator>();
    }
   
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direccion = new Vector3(horizontal, 0f, vertical).normalized;
        if (direccion.magnitude >= 0.1f)
        {
            float anguloDeseado = Mathf.Atan2(direccion.x, direccion.z) * Mathf.Rad2Deg + Camera.main.gameObject.transform.eulerAngles.y;
            float angulo = Mathf.SmoothDampAngle(transform.eulerAngles.y, anguloDeseado, ref velocidadGiro, tiempoRotacion);
            transform.rotation = Quaternion.Euler(0, angulo, 0);

            Vector3 direccionMovimiento = Quaternion.Euler(0f, anguloDeseado, 0f) * Vector3.forward;
            controladorCuerpo.Move(direccionMovimiento.normalized * velocidad * Time.deltaTime);
        }

        if (controladorCuerpo.isGrounded)
        {
            fuerzaGravedad = 0;
            if (Input.GetButton("Jump"))           
                fuerzaGravedad = fuerzaSalto;    
        }
        else {
            fuerzaGravedad -= gravedad * Time.deltaTime; 
        }
        
        controladorCuerpo.Move(new Vector3(0, fuerzaGravedad, 0) * Time.deltaTime);
        EstadosAnimaciones(vertical, horizontal);
    }

    private void EstadosAnimaciones(float vertical, float horizontal)
    {
        float direccion = Mathf.Abs(vertical) + Mathf.Abs(horizontal);
        direccion = Mathf.Clamp(direccion, 0, 1);
        animaciones.SetFloat("Direccion", direccion);
        animaciones.SetBool("EnElPiso", controladorCuerpo.isGrounded);
        if (!controladorCuerpo.isGrounded)
            animaciones.SetInteger("Salto", (int)fuerzaGravedad);
    }
    public void CambiarUbicacion(Transform ubicacion)
    {
        posicionIncial = ubicacion.position;
        rotacionIncial = ubicacion.rotation;
    }
    public void Reiniciar()
    {
        vidas--;
        listaCorazones[vidas].SetActive(false);

        if (vidas == 0)
            murio.Invoke();
        else
        {
            controladorCuerpo.Move(Vector3.zero);
            controladorCuerpo.enabled = false;
            transform.position = posicionIncial;
            transform.rotation = rotacionIncial;
            controladorCuerpo.enabled = true;
        }
    }
}
