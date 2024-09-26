using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] float _jumpPower = 1f;
    [SerializeField] float _hookEjectionForce = 1f;

    [SerializeField] Rigidbody _rig;

    Vector3 _movePower = Vector3.zero;
    bool _onGround = true;

    private void Start()
    {
        _onGround = true;
    }

    private void Update()
    {
        _movePower = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        //ãtå¸Ç´Ç…óÕÇâ¡Ç¶ÇƒìÆÇ´Çé~ÇﬂÇÈ
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            if (_onGround)
            {
                _rig.velocity = Vector3.zero;
            }
        }
        if (_onGround && Input.GetButtonDown("Jump"))
        {
            _movePower += new Vector3(0, _jumpPower, 0);
            _onGround = false;
        }
        //ê⁄ínîªíË
        var line = Physics.Raycast(transform.position, Vector3.down, 1.5f);
        if (line)
        {
            _onGround = true;
        }
        
    }

    private void FixedUpdate()
    {
        _rig.AddForce(_movePower * _moveSpeed, ForceMode.Impulse);
    }
}
