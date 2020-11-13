﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class Character : MonoBehaviour
    {
        [HideInInspector] public bool isFacingLeft;
        [HideInInspector] public bool isGrounded;
        [HideInInspector] public bool isCrouching;
        [HideInInspector] public bool isDashing;

        protected Collider2D col;
        protected Rigidbody2D rb;
        protected Animator anim;
        protected HorizontalMovement movement;

        private Vector2 facingLeft;



        // Start is called before the first frame update
        void Start()
        {
            Initializiation();
        }

        protected virtual void Initializiation()
        {
            col = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            movement = GetComponent<HorizontalMovement>();
            facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

        protected virtual void Flip()
        {
            isFacingLeft = !isFacingLeft;
            if (isFacingLeft)
            {
                transform.localScale = facingLeft;
            }
            else
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            }
        }

        protected virtual bool CollisionCheck(Vector2 direction, float distance, LayerMask collision)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int numHits = col.Cast(direction, hits, distance);
            for (int i = 0; i < numHits; i++)
            {
                if ((1 << hits[i].collider.gameObject.layer & collision) != 0)
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual bool Falling(float velocity)
        {
            //if (!character.isGrounded && rb.velocity.y < velocity)
            if (!isGrounded && rb.velocity.y < velocity)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // this is a kind of gravity
        protected virtual void FallSpeed(float speed)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * speed);
        }
    }
}
