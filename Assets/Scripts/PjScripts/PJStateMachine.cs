using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace uf2
{
    [RequireComponent(typeof(Animator))]
    public class PJStateMachine: MonoBehaviour, IDamageable
    {
        [SerializeField] Hitbox hitbox;
        private Animator _Animator;
        [SerializeField] private InputActionAsset _ActionAsset;
        private InputActionAsset _InputAction;
        private InputAction _MovementAction;

        [SerializeField] private AnimationClip _AttackClip;
        [SerializeField] private AnimationClip _Attack2Clip;
        [SerializeField] private AnimationClip _IdleClip;
        [SerializeField] private AnimationClip _MoveClip;
        [SerializeField] private float _hp;    
        private int _atack1dmg = 2;
        private int _atack2dmg = 4;
        private void Awake()
        {
            _Animator = GetComponent<Animator>();
            Assert.IsNotNull(_ActionAsset, $"No has seleccionat input asset.");

            _InputAction = Instantiate(_ActionAsset);

            _MovementAction = _InputAction.FindActionMap("Player").FindAction("Move");
            _InputAction.FindActionMap("Player").FindAction("Attack").performed += OnAttack;
            _InputAction.FindActionMap("Player").FindAction("Attack2").performed += OnAttackStrong;


            _InputAction.FindActionMap("Player").Enable();

        }

        private enum SkeletonStates { NULL, IDLE, ATTACK, ATTACK2, MOVE, COMBO12, COMBO21 }
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
                    this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    _Animator.Play("PjIdle");
                    break;
                case SkeletonStates.MOVE:
                    _Animator.Play("PjMove");
                    break;

                case SkeletonStates.ATTACK:
                    _Animator.Play("PjAttack");
                    hitbox.Damage = _atack1dmg;
                    break;
                case SkeletonStates.ATTACK2:
                    _Animator.Play("PjAttack2");
                    hitbox.Damage = _atack2dmg;
                    break;
                case SkeletonStates.COMBO12:
                    _Animator.Play("PjAttack2");
                    hitbox.Damage = (int)(_atack2dmg / 0.4f);
                    break;
                case SkeletonStates.COMBO21:
                    _Animator.Play("PjAttack");
                    hitbox.Damage = (int)(_atack2dmg / 0.2f);
                    break;
                default:
                    break;
            }
        }

        private void UpdateState(SkeletonStates updateState)
        {
            Vector2 dir = _MovementAction.ReadValue<Vector2>();
            _StateTime += Time.deltaTime;

            switch (updateState)
            {
                case SkeletonStates.IDLE:
                    if (dir != Vector2.zero)
                        ChangeState(SkeletonStates.MOVE);
                    break;

                case SkeletonStates.MOVE:
                    if (dir == Vector2.zero)
                    {
                        ChangeState(SkeletonStates.IDLE);
                        break;
                    }

                    if (dir.x > 0)
                    {
                        this.transform.eulerAngles = Vector3.zero;
                    }
                    else if (dir.x < 0)
                    {
                        this.transform.eulerAngles = Vector3.up * 180;
                    }

                    this.GetComponent<Rigidbody2D>().velocity = dir * 1;
                    break;


                case SkeletonStates.ATTACK:
                case SkeletonStates.COMBO21:
                    if (_StateTime >= _AttackClip.length)
                        ChangeState(SkeletonStates.IDLE);
                    break;

                case SkeletonStates.ATTACK2:
                case SkeletonStates.COMBO12:
                    if (_StateTime >= _Attack2Clip.length)
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
                case SkeletonStates.MOVE:
                    this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    break;

                case SkeletonStates.ATTACK:
                case SkeletonStates.ATTACK2:
                case SkeletonStates.COMBO12:
                case SkeletonStates.COMBO21:
                    _ComboAvailable = false;
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

                case SkeletonStates.ATTACK2:
                    if (_ComboAvailable)
                        ChangeState(SkeletonStates.COMBO21);
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
                    if (_ComboAvailable)
                        ChangeState(SkeletonStates.COMBO12);
                    break;
                default:
                    break;
            }
        }

        private void Update()
        {
            UpdateState(_CurrentState);
        }

        public void ReceiveDamage(float damage)
        {
            this._hp-=damage;
            if(this._hp <= 0)
                Destroy(this.gameObject);
        }
    }
}
