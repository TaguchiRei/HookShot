using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] float _jumpPower = 1f;
    [SerializeField] float _hookEjectionForce = 1f;
    [SerializeField] float _shotInterval = 0.1f;
    [SerializeField] float _magazineCapacity = 15;

    float _shotIntervalTime = 0;

    [SerializeField] GameObject _bullet;

    [SerializeField] GameObject _fpsHand;
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody _rig;

    Vector3 _movePower = Vector3.zero;
    Dictionary<Anim, string> _anim = new();
    bool _onGround = true;
    bool _gunMode = true;
    int _remainingBullets = 15;

    private void Start()
    {
        _onGround = true;
        _anim.Clear();
        _anim.Add(Anim.run, "run");
        _anim.Add(Anim.reload, "reload");
        _anim.Add(Anim.railgun, "railgun");
        _anim.Add(Anim.aim, "aim");
        _anim.Add(Anim.shootRailgun, "shootRailgun");
    }

    private void Update()
    {
        var hor = Input.GetAxisRaw("Horizontal");
        var ver = Input.GetAxisRaw("Vertical");
        transform.Rotate(0, Input.GetAxisRaw("Mouse X"), 0);
        if (hor != 0 || ver != 0)
        {
            _movePower = new Vector3(ver, 0, hor * -1);
            _animator.SetBool(_anim[Anim.run], true);
        }
        else
        {
            _animator.SetBool(_anim[Anim.run], false);
        }

        if (_onGround && Input.GetButtonDown("Jump"))
        {
            _movePower += new Vector3(0, _jumpPower, 0);
            _onGround = false;
        }
        //�ˌ�
        if (Input.GetButton("Fire1") && _remainingBullets != 0)
        {
            if (_gunMode && _shotIntervalTime <= 0)
            {
                Instantiate(_bullet);
                _shotIntervalTime = _shotInterval;
            }
            else
            {

            }
        }
        if(_remainingBullets == 0)
        {
            if(Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.R))
            {
                _animator.SetBool(_anim[Anim.reload], true);
            }
        }
        
        if (_shotIntervalTime > 0)
        {
            _shotIntervalTime -= Time.deltaTime;
        }
        //�t�b�N�V���b�g�ˌ�
        if (Input.GetButtonDown("Fire2"))
        {

        }
        //�ڒn����
        var line = Physics.Raycast(transform.position, Vector3.down, 1.5f);
        if (line)
        {
            _onGround = true;
        }

    }

    public void Reload()
    {
        _remainingBullets = 15;
        _animator.SetBool(_anim[Anim.reload], false);
    }

    private void FixedUpdate()
    {
        if (_onGround)
        {
            _rig.velocity = transform.TransformDirection(_movePower) * _moveSpeed;
            _movePower = Vector3.zero;
        }
        else
        {
            _rig.AddForce(_movePower * _moveSpeed / 2, ForceMode.Acceleration);
        }
    }
    enum Anim
    {
        run,
        reload,
        railgun,
        aim,
        shootRailgun,
    }
}
