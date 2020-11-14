using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MetroidvaniaTools
{

    public class HorizontalMovement : Abilities
    {
        [SerializeField] protected float timeTillMaxSpeed;
        [SerializeField] protected float maxSpeed;
        [SerializeField] protected float sprintMultiplier;
        [SerializeField] protected float crouchSpeedMultiplier;
        private float acceleration;
        private float currentSpeed;
        private float horizontalInput;
        private float runTime;


        protected override void Initializiation()
        {

            base.Initializiation();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            MovementPressed();
        }

        public virtual bool MovementPressed()
        {
            float _horizontalInput = Input.GetAxis("Horizontal");
            if (_horizontalInput != 0)
            {
                horizontalInput = _horizontalInput;
                return true;
            }
            else
            {
                return false;
            }
        }

        

        protected virtual void FixedUpdate()
        {
            Movement();
        }

        protected virtual void Movement()
        {
            if (MovementPressed())
            {
                anim.SetBool("Moving", true);
                acceleration = maxSpeed / timeTillMaxSpeed;
                runTime += Time.deltaTime;
                currentSpeed = horizontalInput * acceleration * runTime;
                CheckDirection();
                SpeedMultiplier();
            }
            else
            {
                anim.SetBool("Moving", false);
                acceleration = 0;
                runTime = 0;
                currentSpeed = 0;
            }

            anim.SetFloat("CurrentSpeed", currentSpeed);
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        }

        protected void CheckDirection()
        {
            //if (isWallSliding || character.isWallSliding)
            //{
            //    return;
            //}

            if (currentSpeed > 0)
            {
                if (character.isFacingLeft)
                {
                    Flip();
                    isFacingLeft = false;
                    character.isFacingLeft = false;
                }

                if (currentSpeed > maxSpeed)
                {
                    currentSpeed = maxSpeed;
                }
            }
            if (currentSpeed < 0)
            {
                if (!character.isFacingLeft)
                {
                    Flip();
                    isFacingLeft = true;
                    character.isFacingLeft = true;
                }
                if (currentSpeed < -maxSpeed)
                {
                    currentSpeed = -maxSpeed;
                }
            }
            
        }

        protected virtual void SpeedMultiplier()
        {
            
            if (input.SprintingHeld())
            { 
                currentSpeed *= sprintMultiplier;
            }
            if (isCrouching)
            {
                currentSpeed *= crouchSpeedMultiplier;
            }
            if (character.isWallSliding)
            {
                currentSpeed = 0f;
            }
            if ((!character.isFacingLeft && CollisionCheck(Vector2.right, 0.05f, jump.collisionLayer)) || (character.isFacingLeft && CollisionCheck(Vector2.left, 0.05f, jump.collisionLayer)))
            {
                currentSpeed = 0.1f;
            }
        }
    }
}