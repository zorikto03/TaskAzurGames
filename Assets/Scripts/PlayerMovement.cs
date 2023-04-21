using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float JumpSpeed;
    [SerializeField] float JumpCoef;
    [SerializeField] float Gravity = -9.81f;
    [SerializeField] Transform groundCheckerPivot;
    [SerializeField] float checkRadius = 0.1f;
    [SerializeField] LayerMask groundMask;

    [SerializeField] AnimationCurve JumpCurve;
    [SerializeField] DissolveController Dissolve;


    CharacterController controller;
    bool _onGround;
    bool _isJump;
    float _velocityZ;
    bool _isFinished;
    bool _isTouched;

    GameObject _ground;
    float jumpTime, totalJumpTime;

    public static Action GameOver;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        totalJumpTime = JumpCurve[JumpCurve.keys.Length - 1].time;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _onGround)
        {
            _isJump = true;
        }
    }

    void FixedUpdate()
    {
        if (IsOnTheGround())
        {
            _onGround = true;
            _velocityZ = 0;
        }
        else
        {
            _onGround = false;
        }

        if (_isTouched)
        {
            if (!_isFinished)
            {
                Movement();
            }
        }
        DoGravity();
    }

    private void OnEnable()
    {
        Scanner.Finished += Finish;
        DissolveController.FinishedDissolve += PositionReset;
        Touch.TouchedEvent += TouchedEventHandler;
    }

    private void OnDisable()
    {
        Scanner.Finished -= Finish;
        DissolveController.FinishedDissolve -= PositionReset;
        Touch.TouchedEvent -= TouchedEventHandler;
    }

    void TouchedEventHandler()
    {
        _isTouched = true;
        _isJump = true;
    }

    void PositionReset()
    {
        controller.enabled = false;
        controller.transform.position = new Vector3(-48, 27, 0);
        controller.enabled = true;
        GameOver?.Invoke();
    }

    void Finish()
    {
        _isFinished = true;
    }

    void Movement()
    {
        var movement = new Vector3(Speed, 0, 0);
        controller.Move(movement * Speed);

        Jump();
    }

    void Jump()
    {
        if (_isJump)
        {
            if (jumpTime < totalJumpTime)
            {
                var y = JumpCurve.Evaluate(jumpTime);

                controller.Move(Vector3.up * y * JumpCoef);
                //transform.position += pos;
                jumpTime += Time.fixedDeltaTime;
            }
            else
            {
                _isJump = false;
                jumpTime = 0;
            }
        }
    }

    void DoGravity()
    {
        if (!_onGround || !_isJump)
        {
            _velocityZ += Gravity * Time.fixedDeltaTime;

            controller.Move(Vector3.up * _velocityZ);
        }
    }

    bool IsOnTheGround()
    {
        //bool res = Physics.CheckSphere(groundCheckerPivot.position, checkRadius, groundMask);
        Ray ray = new Ray(transform.position, -Vector3.up * checkRadius);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        if (Physics.Raycast(ray, out RaycastHit info, checkRadius))
        {
            switch (info.transform.tag)
            {
                case "Cart":
                    transform.parent = info.transform;
                    return true;
                case "Ground":
                    return true;
                case "GameOverObject":
                    if (!Dissolve.IsPlaying)
                    {
                        Dissolve.PlayAnimation();
                    }
                    return true;
                default:
                    if (_ground != null)
                    {
                        transform.parent = null;
                        _ground = null;
                    }
                    return false;
            }
        }

        return false;
    }
}
