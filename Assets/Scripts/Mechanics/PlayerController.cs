using UnityEngine;
using Platformer.Model;
using Platformer.Core;
using UnityEngine.InputSystem;
using NUnit.Framework;
using System.Collections.Generic;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        // public AudioClip jumpAudio;
        // public AudioClip respawnAudio;
        // public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;

        public float sprintMultiplier = 1.5f;
        private bool _isSprinting = false;

        /*internal new*/
        public Collider2D collider2d;
        /*internal new*/
        public AudioSource audioSource;
        public bool controlEnabled = true;

        Vector2 move;
        private Vector2 _interactionDirection;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        private InputAction m_MoveAction;
        private InputAction m_SprintAction;
        private InputAction m_InteractAction;
        public List<Observer> observers;

        private LayerMask _interactableMask;
        private float _interactionDistance = 1f;
        private bool _hasInteracted = false;

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            m_MoveAction = InputSystem.actions.FindAction("Player/Move");
            m_MoveAction.Enable();

            m_SprintAction = InputSystem.actions.FindAction("Player/Sprint");
            m_SprintAction.Enable();

            m_InteractAction = InputSystem.actions.FindAction("Player/Interact");
            m_InteractAction.Enable();

            _interactableMask = LayerMask.GetMask("Interactable");
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move = m_MoveAction.ReadValue<Vector2>();

                //Save last movement for interaction detection.
                if (move.x != 0 || move.y != 0)
                    _interactionDirection = move;

                _isSprinting = m_SprintAction.ReadValue<float>() > 0.5f;

                Debug.DrawLine(transform.position, transform.position + ((Vector3)_interactionDirection * 3), Color.red);

                if (m_InteractAction.ReadValue<float>() > 0.5f)
                {
                    CheckInteractable();
                    _hasInteracted = true;
                }
                else
                    _hasInteracted = false;
            }
            else
                move = Vector2.zero;

            base.Update();
        }
        protected override void ComputeVelocity()
        {
            //TO DO: Take into account vertical movement as well for sprite changes.
            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            //animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            if (_isSprinting)
            {
                targetVelocity = move * maxSpeed * sprintMultiplier;
                return;
            }

            targetVelocity = move * maxSpeed;
        }

        private void CheckInteractable()
        {

            if (_hasInteracted) return;

            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, _interactionDirection, 3f, _interactableMask);
            if (!rayHit)
                return;

            if (rayHit.distance > _interactionDistance)
                return;

            var interactable = rayHit.transform.gameObject.GetComponent<IInteractable>();
            interactable?.Interact();


        }
    }
}