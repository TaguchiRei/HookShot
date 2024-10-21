using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] float _jumpPower = 1f;
    [SerializeField] float _shotInterval = 0.1f;
    [SerializeField] float _magazineCapacity = 15;
    public bool _canPerspective = false;
    public bool _canMove = false;
    public bool _canJump = false;
    public bool _canShot = false;
    public bool _canDeformation = false;

    float _shotIntervalTime = 0;

    [SerializeField] GameObject _bullet;
    [SerializeField] GameObject _railgunBullet;
    [SerializeField] GameObject _anchor;
    [SerializeField] GameObject _fpsHand;
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody _rig;

    Vector3 _movePower = Vector3.zero;
    Vector3 _hitPos = Vector3.zero;
    GameObject _anc;
    GameObject _manager;
    Dictionary<Anim, string> _anim = new();
    bool _onGround = true;
    bool _usingAnc = false;
    bool _hitAnc = false;
    bool _boost = false;
    float _remainingBullets = 0;
    float _anchorTimer = 0;
    float _cameraSpeed = 1;

    [HideInInspector] public bool _gunMode = true;
    [HideInInspector] public bool _canShootRailgun = false;
    [HideInInspector] public Vector3 _hookShotHitPos;


    private void Start()
    {
        _remainingBullets = _magazineCapacity;
        _manager = FindObjectOfType<GameManager>().gameObject;
        _onGround = true;
        _hitAnc = false;
        _usingAnc = false;
        _cameraSpeed = 1;
        _anim.Clear();
        _anim.Add(Anim.run, "run");
        _anim.Add(Anim.reload, "reload");
        _anim.Add(Anim.railgun, "railgun");
        _anim.Add(Anim.aim, "aim");
        _anim.Add(Anim.shootRailgun, "shootRailgun");
        _anim.Add(Anim.hookShot, "hookShot");
    }

    private void Update()
    {
        //���_����
        if (_canDeformation)
        {
            transform.Rotate(0, Input.GetAxisRaw("Mouse X") * _cameraSpeed, 0);
            var mouseY = Input.GetAxisRaw("Mouse Y");
            _fpsHand.transform.Rotate(0, 0, mouseY * -1 * _cameraSpeed);
        }
        if (_canMove)
        {
            var hor = Input.GetAxisRaw("Horizontal");
            var ver = Input.GetAxisRaw("Vertical");
            if (hor != 0 || ver != 0)
            {
                _movePower = new Vector3(hor, 0, ver);
                if (_onGround)
                {
                    _animator.SetBool(_anim[Anim.run], true);
                }
                else
                {
                    _animator.SetBool(_anim[Anim.run], false);
                }
            }
            else
            {
                _animator.SetBool(_anim[Anim.run], false);
            }
        }
        //�W�����v
        if (_onGround && Input.GetButton("Jump"))
        {
            _rig.linearVelocity = new Vector3(_rig.linearVelocity.x, _jumpPower, _rig.linearVelocity.z);
            _onGround = false;
        }
        //�ˌ�
        if (Input.GetButton("Fire1"))
        {
            if (_gunMode)
            {
                if (_shotIntervalTime <= 0 && _remainingBullets != 0)
                {
                    Instantiate(_bullet);
                    _shotIntervalTime = _shotInterval;
                    _rig.AddForce(transform.forward * -0.5f, ForceMode.Impulse);
                    _remainingBullets--;
                    _manager.GetComponent<GameManager>().ShotGun(_magazineCapacity, _remainingBullets);
                }
                else if (_remainingBullets == 0)
                {
                    _animator.SetBool(_anim[Anim.reload], true);
                }
            }
            else
            {
                if (_canShootRailgun)
                {
                    Instantiate(_railgunBullet);
                    _rig.AddForce((transform.forward * 2 + (_fpsHand.transform.right * -2)) * -100, ForceMode.Impulse);
                    _animator.SetBool(_anim[Anim.shootRailgun], true);
                    _canShootRailgun = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && _remainingBullets != _magazineCapacity)
        {
            _remainingBullets = 0;
            _manager.GetComponent<GameManager>().ShotGun(_magazineCapacity, _remainingBullets);
            _animator.SetBool(_anim[Anim.reload], true);
        }

        if (_shotIntervalTime > 0)
        {
            _shotIntervalTime -= Time.deltaTime;
        }
        //�ό`
        if (Input.GetKeyDown(KeyCode.C))
        {
            _animator.SetBool(_anim[Anim.railgun], true);
            _gunMode = false;

        }
        //�t�b�N�V���b�g�ˌ��ƃ��[���K���\��
        if (Input.GetButtonDown("Fire2"))
        {
            if (_gunMode)
            {
                if (!_usingAnc)
                {
                    _animator.SetBool(_anim[Anim.hookShot], true);
                    _anc = Instantiate(_anchor);
                    _usingAnc = true;
                    _anc.GetComponent<Rigidbody>().linearVelocity = _rig.linearVelocity / 4;
                }
            }
            else
            {
                _cameraSpeed = 0.2f;
                _canShootRailgun = true;
                _animator.SetBool(_anim[Anim.aim], true);
            }
        }
        if (Input.GetButtonUp("Fire2"))
        {
            _cameraSpeed = 1f;
            _animator.SetBool(_anim[Anim.aim], false);
            _canShootRailgun = false;
            Destroy(_anc);
            if (_usingAnc)
            {
                if (_hitAnc)
                {
                    _rig.AddForce(0, 15, 0);
                }
                NotHitAnchor();
            }
        }
        //�A���J�[���O�����߂̃^�C�}�[
        if (_anchorTimer > 0)
        {
            _anchorTimer -= Time.deltaTime;
            if (_anchorTimer < 0)
            {
                _rig.AddForce(0, 15, 0);
                Destroy(_anc);
                NotHitAnchor();
            }
        }
        //�ڒn����
        var line = Physics.Raycast(transform.position, Vector3.down, 1.5f);
        if (line && !_hitAnc)
        {
            _onGround = true;
        }
        else
        {
            _onGround = false;
        }

    }

    /// <summary>
    /// �����[�h�����������Ƃ��̏���
    /// </summary>
    public void Reload()
    {
        _remainingBullets = _magazineCapacity;
        _animator.SetBool(_anim[Anim.reload], false);
        _manager.GetComponent<GameManager>().ShotGun(_magazineCapacity, _remainingBullets);
    }

    /// <summary>
    /// �A���J�[�������������̏���
    /// </summary>
    /// <param name="pos">�A���J�[�����������ꏊ</param>
    public void HitAnchor(Vector3 pos)
    {
        _hitPos = pos;
        _hitAnc = true;
    }

    /// <summary>
    /// �A���J�[��������Ȃ������Ƃ��A�܂��̓A���J�[���O���Ƃ��̏���
    /// </summary>
    public void NotHitAnchor()
    {
        _usingAnc = false;
        _hitAnc = false;
        _boost = false;
        _animator.SetBool(_anim[Anim.hookShot], false);
        _anchorTimer = 0;
        Destroy(_anc);
    }

    private void FixedUpdate()
    {
        if (_onGround)
        {
            Vector3 pMove = transform.TransformDirection(_movePower) * _moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                pMove *= 2f;
            }
            _rig.linearVelocity = new Vector3(pMove.x, _rig.linearVelocity.y, pMove.z);
            _movePower = Vector3.zero;
        }
        else
        {
            _rig.AddForce(_moveSpeed * 3 * transform.TransformDirection(_movePower), ForceMode.Acceleration);
        }

        //�d�͂����
        if (_rig.linearVelocity.y < 1)
        {
            _rig.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
        }
        else
        {
            _rig.AddForce(1.5f * 9.81f * Vector3.down, ForceMode.Acceleration);
        }
        //�t�b�N�V���b�g�̓��������
        if (_usingAnc && _hitAnc)
        {
            Vector3 vec = _hitPos - transform.position;
            vec.Normalize();
            if (!_boost)
            {
                _boost = true;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _rig.linearVelocity = vec * 10;
                }
                else
                {
                    _rig.linearVelocity *= 0.8f;
                }
                _anchorTimer = 10;
                _onGround = false;
            }
            else
            {
                _rig.AddForce(vec * 60f, ForceMode.Acceleration);
            }
        }
    }

    /// <summary>
    /// �A�j���[�V�������Ǘ����邽�߂̕ϐ����������͂łł���悤��enum���g�p
    /// </summary>
    enum Anim
    {
        run,
        reload,
        railgun,
        aim,
        shootRailgun,
        hookShot,
    }
}
