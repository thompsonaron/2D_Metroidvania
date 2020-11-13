using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{

    public class Jump : Abilities
    {
        [SerializeField] protected bool limitAirJumps;
        [SerializeField] protected int maxNumberOfJumps;
        [SerializeField] protected float jumpForce;
        [SerializeField] protected float holdForce;
        [SerializeField] protected float buttonHoldTime;
        [SerializeField] protected float distanceToColllider;
        [SerializeField] protected float horizontalWallJumpForce;
        [SerializeField] protected float verticalWallJumpForce;
        [SerializeField] protected float maxJumpSpeed;
        [SerializeField] protected float maxFallSpeed;
        [SerializeField] protected float acceptedFallSpeed;
        [SerializeField] protected float glideTime;
        [SerializeField] [Range(-2, 2)] protected float gravity;
        public LayerMask collisionLayer;

        private bool isJumping;
        private bool isWallJumping;
        private float jumpCountDown;
        private float fallCountDown;
        private int numberOfJumpsLeft;

        protected override void Initializiation()
        {
            base.Initializiation();

            numberOfJumpsLeft = maxNumberOfJumps;
            jumpCountDown = buttonHoldTime;
            fallCountDown = glideTime;
        }

        protected virtual void Update()
        {
            JumpPressed();
            JumpHeld();
        }

        protected virtual void FixedUpdate()
        {
            IsJumping();
            Gliding();
            GroundCheck();
            WallSliding();
            WallJump();
        }

        protected virtual bool JumpHeld()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                return true;
            }
            else
            {
                return false;
            }
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

                if (limitAirJumps && Falling(acceptedFallSpeed))
                {
                    isJumping = false;
                    return false;
                }
                // wall jumping
                if (character.isWallSliding)
                {
                    isWallJumping = true;
                    return false;
                }
                // real code
                numberOfJumpsLeft--;
                if (numberOfJumpsLeft >= 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    jumpCountDown = buttonHoldTime;
                    isJumping = true;
                    fallCountDown = glideTime;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual void IsJumping()
        {

            if (isJumping)
            {
                rb.AddForce(Vector2.up * jumpForce);
                AdditionallAir();
            }
            if (rb.velocity.y > maxJumpSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxJumpSpeed);
            }
        }

        protected virtual void Gliding()
        {
            if (Falling(0) && JumpHeld())
            {
                fallCountDown -= Time.deltaTime;
                if (fallCountDown > 0 && rb.velocity.y > acceptedFallSpeed)
                {
                    FallSpeed(gravity);
                    anim.SetBool("Gliding", true);
                    return;
                }
            }
            anim.SetBool("Gliding", false);
        }

        protected virtual void AdditionallAir()
        {
            if (JumpHeld())
            {
                jumpCountDown -= Time.deltaTime;
                if (jumpCountDown <= 0)
                {
                    jumpCountDown = 0;
                    isJumping = false;
                }
                else
                {
                    rb.AddForce(Vector2.up * holdForce);
                }
            }
            else
            {
                isJumping = false;
            }
        }

        private void GroundCheck()
        {
            if (CollisionCheck(Vector2.down, distanceToColllider, collisionLayer) && !isJumping)
            {
                character.isGrounded = true;
                numberOfJumpsLeft = maxNumberOfJumps;
                fallCountDown = glideTime;
            }
            else
            {
                character.isGrounded = false;
                if (Falling(0) && rb.velocity.y < maxFallSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
                }
            }
            anim.SetBool("IsGrounded", character.isGrounded);
            anim.SetFloat("VerticalSpeed", rb.velocity.y);
        }

        protected virtual bool WallSliding()
        {
            
            if (WallCheck())
            {
                // gravity is glide speed
                FallSpeed(gravity);
                character.isWallSliding = true;
                return true;
            }
            else
            {
                character.isWallSliding = false;
                return false;
            }
        }

        protected virtual bool WallCheck()
        {
            if ((!character.isFacingLeft && CollisionCheck(Vector2.right, distanceToColllider, collisionLayer)) || (character.isFacingLeft && CollisionCheck(Vector2.left, distanceToColllider, collisionLayer)) && movement.MovementPressed() && !character.isGrounded)
            {
                return true;
            }
            return false;
        }

        protected virtual void WallJump()
        {
            if (isWallJumping)
            {
                rb.AddForce(Vector2.up * verticalWallJumpForce);
                if (!character.isFacingLeft)
                {
                    rb.AddForce(Vector2.right * horizontalWallJumpForce);
                }
                if (character.isFacingLeft)
                {
                    rb.AddForce(Vector2.left * horizontalWallJumpForce);
                }
            }
        }
    }
}
