using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform jogador;
    public float camDistancia = 30.0f;

    void Awake(){
        //mudar a distancia da camera
        GetComponent<UnityEngine.Camera>().orthographicSize = ((Screen.height/2)/camDistancia);
    }

    void FixedUpdate(){
        //x muda a posicao da camera
        transform.position = new Vector3(jogador.position.x, transform.position.y, transform.position.z);
    }
}
