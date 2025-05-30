using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

namespace _Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float turnSpeed;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float jumpForce = 300f;
        [SerializeField] private PlayerStats playerStats;

        private PlayerInput _playerInput;
        private Rigidbody _rigidbody;
        private Animator _animator;
        private bool _isGrounded;
        private bool _isHit;
        private Obstacle _obstacle;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            Move(_playerInput.InputVector);
            Rotate(_playerInput.InputQuaternion);
            Jump();

            _animator.SetBool("isGrounded", _isGrounded);
            _animator.SetBool("isHit", _isHit);
            //_animator.SetBool("isRunning", _playerInput.InputVector.magnitude > 0.1f);
        }

        private void Move(Vector3 direction)
        {
            _rigidbody.MovePosition(_rigidbody.position + direction * (turnSpeed * Time.fixedDeltaTime));
        }

        private void Rotate(Quaternion angle)
        {
            transform.rotation = angle;
        }

        private void Jump()
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.01f, groundMask);
            if (_playerInput.IsJumping && _isGrounded)
            {
                _rigidbody.AddForce(Vector3.up * jumpForce);
                //_animator.Play("Jump");
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            var obj = collision.gameObject;
            if (obj.CompareTag("Obstacle"))
            {
                _animator.applyRootMotion = true;
                Hit(true);
                //transform.DOMove(transform.position + transform.forward * 0.8f, 1.2f).SetEase(Ease.OutQuad);
                playerStats.SetHit();
                _obstacle = obj.GetComponent<Obstacle>();
                _animator.Play("Hit", 0);
            }
        }

        public void Hit(bool isHit)
        {
            _isHit = isHit;
        }

        public void Respawn(bool loose)
        {
            // Сбросить позицию и поворот
            var currentPosition = transform.position;
            transform.position = new Vector3(currentPosition.x, 0f, currentPosition.z);
            transform.rotation = Quaternion.identity;

            // Остановить физику
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.Sleep(); // Полный сброс физики (опционально)

            Hit(false);
            if (_obstacle != null) _obstacle.Reset();
            _obstacle = null;

            _isGrounded = true;

            // Сбросить ввод
            _playerInput.ResetInput(); // <- убедись, что этот метод реально обнуляет InputVector и IsJumping

            // Сбросить анимации
            _animator.Rebind();
            _animator.Update(0f);
            _animator.Play("Fast Run", 0); // Явно задать стартовую анимацию

            // Сбросить root motion, если включался при ударе
            _animator.applyRootMotion = false;

        }
    }
}
