using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0, 180f * Time.deltaTime, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        FindObjectOfType<UIController>().Animate(transform.position);
        Destroy(gameObject);
    }
}
