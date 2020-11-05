using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{

    public class Jump : Abilities
    {
        [SerializeField] protected int maxNumberOfJumps;
        [SerializeField] protected float jumpForce;
        [SerializeField] protected float distanceToColllider;
        [SerializeField] protected LayerMask collisionLayer;

        private bool isJumping;
        private int numberOfJumpsLeft;

        protected override void Initializiation()
        {
            base.Initializiation();

            numberOfJumpsLeft = maxNumberOfJumps;
        }

        protected virtual void Update()
        {
            JumpPressed();
        }

        protected virtual bool JumpPressed()
        {
             if (Input.GetKeyDown(KeyCode.Space))
            {
                // check if falling and havent jumped
                if (!character.isGrounded && numberOfJumpsLeft == maxNumberOfJumps)
                {
                    isJumping = false;
                    return false;
                }

                // real code
                numberOfJumpsLeft--;
                if (numberOfJumpsLeft >= 0)
                {
                    isJumping = true;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void FixedUpdate()
        {
            IsJumping();
            GroundCheck();
        }

        protected virtual void IsJumping()
        {
            
            if (isJumping)
            {
                rb.AddForce(Vector2.up * jumpForce);
            }
        }

        private void GroundCheck()
        {
            if (CollisionCheck(Vector2.down, distanceToColllider, collisionLayer) && !isJumping)
            {
                character.isGrounded = true;
                numberOfJumpsLeft = maxNumberOfJumps;
            }
            else
            {
                character.isGrounded = false;
                isJumping = false;
            }
        }
    }
}
