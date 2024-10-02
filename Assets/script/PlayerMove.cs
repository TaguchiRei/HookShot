using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] float _jumpPower = 1f;
    [SerializeField] float _hookEjectionForce = 1f;
    [SerializeField] float shotInterval = 0.1f;
    float shotIntervalTime = 0;

    [SerializeField] GameObject _bullet;

    [SerializeField] GameObject _fpsHand;
    [SerializeField] MakePendulum _makePendulum;
    [SerializeField] Rigidbody _rig;

    Vector3 _movePower = Vector3.zero;
    bool _onGround = true;
    bool _gunMode = true;

    private void Start()
    {
        _onGround = true;
    }

    private void Update()
    {
        _movePower = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        
        if (_onGround && Input.GetButtonDown("Jump"))
        {
            _movePower += new Vector3(0, _jumpPower, 0);
            _onGround = false;
        }
        //射撃
        if (Input.GetButton("Fire1"))
        {
            if(_gunMode && shotIntervalTime <= 0)
            {
                Instantiate(_bullet);
                shotIntervalTime = shotInterval;
            }
            else
            {

            }
        }
        if(shotIntervalTime > 0)
        {
            shotIntervalTime -= Time.deltaTime;
        }
        //フックショット射撃
        if (Input.GetButtonDown("Fire2"))
        {

        }
        //接地判定
        var line = Physics.Raycast(transform.position, Vector3.down, 1.5f);
        if (line)
        {
            _onGround = true;
        }
        
    }

    private void FixedUpdate()
    {
        if(_onGround)
        {
            _rig.AddForce(_movePower * _moveSpeed, ForceMode.Impulse);
            var A = _rig.velocity / (_moveSpeed / Mathf.Abs(_rig.velocity.x) + Mathf.Abs(_rig.velocity.y) + Mathf.Abs(_rig.velocity.z));
            if (Mathf.Abs(_rig.velocity.x) + Mathf.Abs(_rig.velocity.y) + Mathf.Abs(_rig.velocity.z) < 10)
            {
                _rig.velocity = new Vector3();
            }
        }
        else
        {
            _rig.AddForce(_movePower * _moveSpeed / 2, ForceMode.Acceleration);
        }
    }
}
