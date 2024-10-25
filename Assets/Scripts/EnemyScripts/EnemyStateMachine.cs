using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace uf2
{
    [RequireComponent(typeof(Animator))]
    public class EnemyStateMachine: MonoBehaviour, IDamageable
    {
        [SerializeField] public EnemySO _enemySO;
        private Animator _animator;
        [SerializeField] private Hitbox hitbox;
        [SerializeField] private RangeDetection _rangPerseguir;
        [SerializeField] private RangeDetection _rangAtac;
        [SerializeField] private Knife[] knifes;
        
        void OnEnable()
        {
            _animator = GetComponent<Animator>();
            this.hp = _enemySO.hp;
            this._rangAtac.GetComponent<CircleCollider2D>().radius = this._enemySO.rangeAttack;
            _rangPerseguir.OnEnter += PerseguirDetected;
            _rangPerseguir.OnStay += PerseguirDetected;
            _rangPerseguir.OnExit += PerseguirUndetected;
            _rangAtac.OnEnter += AtacarDetected;
            _rangAtac.OnStay += AtacarDetected;
            _rangAtac.OnExit += AtacarUndetected;
        }
        private void Awake()
        {

        }

        private enum SkeletonStates { NULL, IDLE, ATTACK, ATTACK2, MOVE, COMBO12, COMBO21 }
        [SerializeField] private SkeletonStates _CurrentState;
        [SerializeField] private float _StateTime;
        [SerializeField] private float hp;

        public event Action<float> OnDamaged;


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
                    _animator.Play("RedSkeletonIdle");
                    break;
                case SkeletonStates.MOVE:
                    _animator.Play("RedSkeletonMove");
                    break;

                case SkeletonStates.ATTACK:
                    _animator.Play("RedSkeletonAttack");
                    hitbox.Damage = _enemySO.dmg;
                    break;
                case SkeletonStates.ATTACK2:
                    _animator.Play("RedSkeletonAttack2");
                    break;
                default:
                    break;
            }
        }

        private void UpdateState(SkeletonStates updateState)
        {
            Vector2 dir = this.GetComponent<Rigidbody2D>().velocity;
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
                    if (_StateTime >= _enemySO.clipAttack.length)
                        ChangeState(SkeletonStates.IDLE);
                    break;
                case SkeletonStates.ATTACK2:
                    if (_StateTime >= _enemySO.clipAttack2.length)
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
            this.hp -= damage;
            if (this.hp <= 0)
            {
                this.gameObject.SetActive(false);
                _rangPerseguir.OnEnter -= PerseguirDetected;
                _rangPerseguir.OnStay -= PerseguirDetected;
                _rangPerseguir.OnExit -= PerseguirUndetected;
                _rangAtac.OnEnter -= AtacarDetected;
                _rangAtac.OnStay -= AtacarDetected;
                _rangAtac.OnExit -= AtacarUndetected;
            }
        }
        private void PerseguirDetected(GameObject personatge)
        {
            if (personatge.name == "PJ" && _enemySO.clipAttack.length <= _StateTime)
            {
                this.GetComponent<Rigidbody2D>().velocity = (personatge.gameObject.transform.position - this.transform.position).normalized;
            }
        }
        private void PerseguirUndetected(GameObject personatge)
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        public bool cooldown = false;
        private void AtacarDetected(GameObject personatge)
        {
            if (personatge.name == "PJ")
            {
                this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                if (this.GetComponent<SpriteRenderer>().color == Color.white || this.GetComponent<SpriteRenderer>().color == Color.red)
                    ChangeState(SkeletonStates.ATTACK);
                else
                {
                    if (!cooldown)
                    {
                        ChangeState(SkeletonStates.ATTACK2);
                        cooldown = true;
                        StartCoroutine(cooldownFalse());
                    }
                }
            }
        }
        IEnumerator cooldownFalse()
        {
            yield return new WaitForSeconds(1.5f);
            cooldown = false;
        }
        private void spawnKife()
        {
            for(int x = 0; x < knifes.Length; x++)
            {
                if (!knifes[x].gameObject.activeSelf)
                {
                    knifes[x].Damage = (int)(_enemySO.dmg2);
                    knifes[x].gameObject.transform.position =  this.transform.position;
                    knifes[x].gameObject.SetActive(true);
                    break;
                }
            }
        }
        private void AtacarUndetected(GameObject personatge)
        {

        }
    }


}
