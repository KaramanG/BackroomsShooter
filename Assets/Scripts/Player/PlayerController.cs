using UnityEngine;

namespace BackroomsShooter.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement settings")]
        public float MoveSpeed = 5f;
        public float DashForce = 10f;
        public float DashCooldown = 1f;
        public float DashStaminaConsumption = 30f;

        private Rigidbody _rb;
        private Vector3 _moveInput;
        private Vector3 _mousePos;
        
        private Camera _mainCamera;
        private PlayerResources _resources;
        private float _nextDashTime;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _mainCamera = Camera.main;
            _resources = GetComponent<PlayerResources>();
        }

        private void Update()
        {
            _moveInput.x = Input.GetAxisRaw("Horizontal");
            _moveInput.z = Input.GetAxisRaw("Vertical");

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _mousePos = hit.point;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= _nextDashTime && !_resources.IsReloading)
            {
                PerformDash();
            }
        }

        private void FixedUpdate()
        {
            float currentSpeed = MoveSpeed;
            if (_resources != null && _resources.IsReloading) currentSpeed *= 0.4f;

            _rb.MovePosition(_rb.position + _moveInput.normalized * currentSpeed * Time.fixedDeltaTime);

            Vector3 lookDir = _mousePos - transform.position;
            lookDir.y = 0;
            if (lookDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDir);
            }
        }

        private void PerformDash()
        {
            Vector3 dashDir = _moveInput.normalized;

            if (dashDir == Vector3.zero) return;

            if (_resources.ConsumeStamina(DashStaminaConsumption))
            {
                _rb.AddForce(dashDir * DashForce, ForceMode.Impulse);
                _nextDashTime = Time.time + DashCooldown;
            }
        }

    }
}