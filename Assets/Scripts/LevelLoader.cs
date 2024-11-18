using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float TransitionTime = 1f;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.anyKeyDown)
        {
            StartCoroutine(CarregarFase("Fase1"));
        }
    }

    //Corrotina - Coroutine
    IEnumerator CarregarFase(string NomeFase)
    {
        //Iniciar anima��o
        transition.SetTrigger("Start");


        //Espera o tempode anima��o
        yield return new WaitForSeconds(TransitionTime);

        //Carregar proxima cena
        SceneManager.LoadScene(NomeFase);
    }
}
