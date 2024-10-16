using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using static UnityEditor.VersionControl.Asset;

namespace uf2
{
    [RequireComponent(typeof(Animator))]
    public class SkeletonStateMachineSwitch : MonoBehaviour
    {
        private Animator _Animator;
        [SerializeField] private InputActionAsset _ActionAsset;
        private InputActionAsset _InputAction;

        [SerializeField] private AnimationClip _AttackClip;
        [SerializeField] private AnimationClip _Attack2Clip;
        [SerializeField] private AnimationClip _IdleClip;
        [SerializeField] private AnimationClip _MoveClip;
        private void Awake()
        {
            _Animator = GetComponent<Animator>();
            Assert.IsNotNull(_ActionAsset, $"No has seleccionat input asset.");

            _InputAction = Instantiate(_ActionAsset);

            _InputAction.FindActionMap("Player").FindAction("Move").performed += OnMovement;
            _InputAction.FindActionMap("Player").FindAction("Attack").performed += OnAttack;
            _InputAction.FindActionMap("Player").FindAction("Attack2").performed += OnAttackStrong;


            _InputAction.FindActionMap("Player").Enable();

        }

        private enum SkeletonStates { NULL, IDLE, ATTACK, ATTACK2, MOVE }
        [SerializeField] private SkeletonStates _CurrentState;
        [SerializeField] private float _StateTime;
        private bool _ComboAvailable;

        public void StartCombo()
        {
            _ComboAvailable = true;
        }

        public void EndCombo()
        {
            _ComboAvailable = false;
        }

        private void Start()
        {
            ChangeState(SkeletonStates.IDLE);
        }

        private void ChangeState(SkeletonStates newState)
        {
            //tornar al mateix estat o no
            if (newState == _CurrentState)
                return;

            ExitState(_CurrentState);
            InitState(newState);
        }

        private void InitState(SkeletonStates initState)
        {
            _CurrentState = initState;
            _StateTime = 0f;

            switch (_CurrentState)
            {
                case SkeletonStates.IDLE:
                    _Animator.Play("PjIdle");
                    break;
                case SkeletonStates.ATTACK:
                    _Animator.Play("PjAttack");
                    break;
                case SkeletonStates.ATTACK2:
                    _Animator.Play("PjAttack2");
                    break;
                case SkeletonStates.MOVE:
                    _Animator.Play("PjMove");
                    break;
                default:
                    break;
            }
        }


        private void UpdateState(SkeletonStates updateState)
        {
            Vector2 dir = _InputAction.FindActionMap("Player").FindAction("Move").ReadValue<Vector2>();
            _StateTime += Time.deltaTime;

            switch (updateState)
            {
                case SkeletonStates.IDLE:
                    break;

                case SkeletonStates.ATTACK:
                    if (_StateTime >= _AttackClip.length)
                        ChangeState(SkeletonStates.IDLE);
                    break;

                case SkeletonStates.ATTACK2:
                    if (_StateTime >= _Attack2Clip.length)
                        ChangeState(SkeletonStates.IDLE);
                    break;
                case SkeletonStates.MOVE:
                    if (dir.x != 0 )
                        ChangeState(SkeletonStates.IDLE);
                    break;
                default:
                    break;
            }
        }

        private void ExitState(SkeletonStates exitState)
        {
            switch (exitState)
            {
                case SkeletonStates.IDLE:
                    break;
                case SkeletonStates.ATTACK:
                    _ComboAvailable = false;
                    break;
                case SkeletonStates.ATTACK2:
                    break;
                case SkeletonStates.MOVE:
                    break;
                default:
                    break;
            }
        }
        private void OnAttack(InputAction.CallbackContext context)
        {
            switch (_CurrentState)
            {
                case SkeletonStates.IDLE:
                case SkeletonStates.MOVE:
                    ChangeState(SkeletonStates.ATTACK);
                    break;
                case SkeletonStates.ATTACK:
                    if (_ComboAvailable)
                        ChangeState(SkeletonStates.ATTACK2);
                    break;
                case SkeletonStates.ATTACK2:
                    break;
                default:
                    break;
            }
        }
        private void OnAttackStrong(InputAction.CallbackContext context)
        {
            switch (_CurrentState)
            {
                case SkeletonStates.IDLE:
                case SkeletonStates.MOVE:
                    ChangeState(SkeletonStates.ATTACK2);
                    break;
                case SkeletonStates.ATTACK:
                    break;
                case SkeletonStates.ATTACK2:
                    break;
                default:
                    break;
            }
        }
        private void OnMovement(InputAction.CallbackContext context)
        {
            switch (_CurrentState)
            {
                case SkeletonStates.IDLE:
                    ChangeState(SkeletonStates.MOVE);
                    break;
                case SkeletonStates.ATTACK:
                    break;
                case SkeletonStates.ATTACK2:
                    break;
                default:
                    break;
            }
        }
        private void Update()
        {
            UpdateState(_CurrentState);
        }

    }
}
