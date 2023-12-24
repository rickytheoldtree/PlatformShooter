using System.Collections.Generic;
using UnityEngine;

namespace PlatformShooter.Character
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Physics2DBase : MonoBehaviour
    {
        public float GravityMod { get; set; } = 1f;
        protected Vector2 alternativeForce;
        protected Rigidbody2D rb;
        protected Vector2 targetVelocity;
        protected Vector2 velocity;
        protected const float MinMoveDistance = 0.001f;
        protected ContactFilter2D contactFilter;
        protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
        protected const float MinGroundNormalY = 0.65f;
        protected readonly List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
        protected Vector2 groundNormal;
        protected bool isGrounded;
        protected const float ShellRadius = 0.01f;
        protected float airTime;
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            contactFilter.useTriggers = false;
            contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            contactFilter.useLayerMask = true;
        }
        protected virtual void Update()
        {
            targetVelocity = Vector2.zero;
            ComputeGravity();
            ComputeVelocity();
        }
        private void FixedUpdate()
        {
            velocity += Physics2D.gravity * (GravityMod  * Time.deltaTime);
            velocity.x = targetVelocity.x;
            isGrounded = false;
            var deltaPosition = velocity * Time.deltaTime;
            var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
            var move = moveAlongGround * deltaPosition.x;
            Move(move, false);
            move = Vector2.up * deltaPosition.y;
            Move(move, true);
            if (!isGrounded) airTime += Time.deltaTime;
            else airTime = 0;
        }
        protected virtual void ComputeVelocity() { }
        protected virtual void ComputeGravity() { }
        private void Move (Vector2 move, bool yAxis)
        {
            var distance = move.magnitude;
            if (distance > MinMoveDistance)
            {
                var count = rb.Cast(move, contactFilter, hitBuffer, distance + ShellRadius);
                hitBufferList.Clear();
                for (var i = 0; i < count; i++)
                {
                    var hit = hitBuffer[i];
                    hitBufferList.Add(hit);
                }
                foreach (var hit in hitBufferList)
                {
                    var currentNormal = hit.normal;
                    if (currentNormal.y > MinGroundNormalY)
                    {
                        isGrounded = true;
                        airTime = 0;
                        if (yAxis)
                        {
                            groundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }
                    var projection = Vector2.Dot(velocity, currentNormal);
                    if (projection < 0)
                    {
                        velocity -= projection * currentNormal;
                    }
                    var modifiedDistance = hit.distance - ShellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
            rb.position += move.normalized * distance;
        }

        public void AddForce(Vector2 force)
        {
            alternativeForce += force;
        }
    }
}