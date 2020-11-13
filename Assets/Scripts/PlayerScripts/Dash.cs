﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class Dash : Abilities
    {
        [SerializeField] protected float dashForce;
        [SerializeField] protected float dashCoolDownTime;
        [SerializeField] protected float dashAmountTime;
        [SerializeField] protected LayerMask dashingLayers;

        private bool canDash;
        private float dashCountDown;

        protected virtual void Update()
        {
            DashPressed();
        }

        protected virtual void FixedUpdate()
        {
            DashMode();
            ResetDashCounter();

        }

        protected virtual void DashMode()
        {
            if (character.isDashing == true)
            {
                FallSpeed(0);
                movement.enabled = false;

                if (!character.isFacingLeft)
                {
                    DashCollision(Vector2.right, 0.5f, dashingLayers);
                    rb.AddForce(Vector2.right * dashForce);
                }
                else
                {
                    DashCollision(Vector2.left, 0.5f, dashingLayers);
                    rb.AddForce(Vector2.left * dashForce);
                }
            }
        }

        protected virtual bool DashPressed()
        {
            if (Input.GetKeyDown(KeyCode.Z) && canDash)
            {
                Dashing();
                return true;
            }
            else
            {
            return false;
            }
        }

        protected virtual void Dashing()
        {
            dashCountDown = dashCoolDownTime;
            character.isDashing = true;
            StartCoroutine(FinishedDashing());
        }

        protected virtual void DashCollision(Vector2 direction, float distance, LayerMask collision)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int numHits = col.Cast(direction, hits, distance);
            for (int i = 0; i < numHits; i++)
            {
                if ((1 << hits[i].collider.gameObject.layer & collision) != 0)
                {
                    hits[i].collider.enabled = false;
                    StartCoroutine(TurnColliderBackOn(hits[i].collider.gameObject));
                }
            }
        }

        protected virtual void ResetDashCounter()
        {
            if (dashCountDown > 0)
            {
                canDash = false;
                dashCountDown -= Time.deltaTime;
            }
            else
            {
                canDash = true;
            }
        }

        protected virtual IEnumerator FinishedDashing()
        {
            yield return new WaitForSeconds(dashAmountTime);
            character.isDashing = false;
            FallSpeed(1);
            movement.enabled = true;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        protected virtual IEnumerator TurnColliderBackOn(GameObject obj)
        {
            yield return new WaitForSeconds(dashAmountTime);
            obj.GetComponent<Collider2D>().enabled = true;
        }
    }
}