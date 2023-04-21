using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    CartMovement cart;
    Vector3 startPosCart;

    private void Start()
    {
        cart = FindObjectOfType<CartMovement>();
        startPosCart = cart.transform.position;
    }


    private void OnEnable()
    {
        PlayerMovement.GameOver += Restart;
        Scanner.Finished += Finish;
    }

    private void OnDisable()
    {
        PlayerMovement.GameOver -= Restart;
        Scanner.Finished -= Finish;
    }


    void Restart()
    {
        cart.transform.position = startPosCart;
    }

    void Finish()
    {
        Debug.Log("Finish");
    }
}
