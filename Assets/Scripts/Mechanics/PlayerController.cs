using UnityEngine;
using Platformer.Model;
using Platformer.Core;
using UnityEngine.InputSystem;

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

        private PlayerInput _playerInput;
        private LayerMask _interactableMask;
        public float InteractionDistance = 1.5f;

        private StartDialogue _interactedDialogue;
        private InteractableSequence _interactableSequence;
        
        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            _playerInput = GetComponent<PlayerInput>();

            _interactableMask = LayerMask.GetMask("Interactable");
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move = _playerInput.actions["Move"].ReadValue<Vector2>();

                //Save last movement for interaction detection.
                if (move.x != 0 || move.y != 0)
                    _interactionDirection = move;

                _isSprinting = _playerInput.actions["Sprint"].ReadValue<float>() > 0.5f;

                Debug.DrawLine(transform.position, transform.position + ((Vector3)_interactionDirection * 3), Color.red);
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
                targetVelocity = move * (maxSpeed * sprintMultiplier);
                return;
            }

            targetVelocity = move * maxSpeed;
        }

        public void CheckInteractable(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, _interactionDirection, 3f, _interactableMask);
            if (!rayHit)
                return;

            if (rayHit.distance > InteractionDistance)
                return;

            var interactables = rayHit.transform.gameObject.GetComponents<IInteractable>();

            _interactedDialogue = rayHit.transform.gameObject.GetComponent<StartDialogue>();
            _interactableSequence = rayHit.transform.gameObject.GetComponent<InteractableSequence>();
            foreach(var interactable in interactables)
            {
                interactable?.Interact();
            }
        }

        public void OnProgressDialogue(InputAction.CallbackContext context)
        {
            if (_interactedDialogue == null || !context.started) return;
            
            if (!_interactedDialogue.Progress()) return;
            
            _interactedDialogue = null;
        }
        
        public void OnProgressSequence(InputAction.CallbackContext context)
        {
            if (_interactableSequence == null || !context.started) return;
            
            if (!_interactableSequence.Progress()) return;
            
            _interactableSequence = null;
        }
    }
}