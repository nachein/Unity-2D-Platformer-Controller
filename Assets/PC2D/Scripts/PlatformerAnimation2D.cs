﻿using UnityEngine;

namespace PC2D
{
    /// <summary>
    /// This is a very very very simple example of how an animation system could query information from the motor to set state.
    /// This can be done to explicitly play states, as is below, or send triggers, float, or bools to the animator. Most likely this
    /// will need to be written to suit your game's needs.
    /// </summary>

    public class PlatformerAnimation2D : MonoBehaviour
    {
        public float jumpRotationSpeed;
        public GameObject visualChild;

        private PlatformerMotor2D _motor;
        private SimpleAI _ai;
        private Animator _animator;
        private bool _isJumping;

        // Use this for initialization
        void Start()
        {
            _motor = GetComponent<PlatformerMotor2D>();
            _ai = GetComponent<SimpleAI>();
            _animator = visualChild.GetComponent<Animator>();
            _animator.Play("Idle");
        }

        // Update is called once per frame
        void Update()
        {
            if (_motor.motorState == PlatformerMotor2D.MotorState.Jumping ||
                _isJumping &&
                    (_motor.motorState == PlatformerMotor2D.MotorState.Falling ||
                                 _motor.motorState == PlatformerMotor2D.MotorState.FallingFast))
            {
                _isJumping = true;
                _animator.Play("Jump");
                visualChild.transform.Rotate(Vector3.back, jumpRotationSpeed * Time.deltaTime);
            }
            else
            {
                _isJumping = false;
                visualChild.transform.rotation = Quaternion.identity;

                if (_motor.motorState == PlatformerMotor2D.MotorState.Falling ||
                                 _motor.motorState == PlatformerMotor2D.MotorState.FallingFast)
                {
                    _animator.Play("Fall");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.WallSliding ||
                         _motor.motorState == PlatformerMotor2D.MotorState.WallSticking ||
                        _motor.motorState == PlatformerMotor2D.MotorState.OnCorner)
                {
                    _animator.Play("Cling");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.Slipping)
                {
                    _animator.Play("Slip");
                }
                else if (_motor.motorState == PlatformerMotor2D.MotorState.Dashing)
                {
                    _animator.Play("Dash");
                }
                else
                {
                    if (_motor.velocity.sqrMagnitude >= 0.1f * 0.1f)
                    {
                        _animator.Play("Walk");
                    }
                    else
                    {
                        _animator.Play("Idle");
                    }
                }
            }

            if (!_isJumping)
            {
                // Facing
                float valueCheck;

                if (_ai != null)
                {
                    valueCheck = _ai.movement;
                }
                else
                {
                    valueCheck = UnityEngine.Input.GetAxisRaw(PC2D.Input.HORIZONTAL);
                }

                if (_motor.motorState == PlatformerMotor2D.MotorState.Slipping ||
                    _motor.motorState == PlatformerMotor2D.MotorState.Dashing)
                {
                    valueCheck = _motor.velocity.x;
                }

                if (valueCheck >= 0.1f)
                {
                    transform.localScale = Vector3.one;
                }
                else if (valueCheck <= -0.1f)
                {
                    Vector3 newScale = Vector3.one;
                    newScale.x = -1;
                    transform.localScale = newScale;
                }
            }
        }
    }
}
