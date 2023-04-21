using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMovement : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float CheckRadius = 5f;
    bool _move = false;
    //Rigidbody rb;

    private void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_move)
        {
            Move();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player exit the Cart");

            _move = false;
        }
    }

    private void Move()
    {
        Ray ray = new Ray(transform.position, Vector3.right * CheckRadius);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        if (Physics.Raycast(ray, out RaycastHit info, CheckRadius))
        {
            _move = false;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position , new Vector3(75, 0, 0), Speed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player enter to the Cart");

            _move = true;
        }
    }

    public void StartMove()
    {
        _move = true;
    }

    public void StopMove()
    {
        _move = false;
    }
}
