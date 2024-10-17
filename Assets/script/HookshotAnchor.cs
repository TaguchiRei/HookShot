using UnityEngine;

public class HookshotAnchor : MonoBehaviour
{
    [SerializeField] Material _ropeMaterial;
    [SerializeField] Rigidbody _rig;
    [SerializeField] LineRenderer _lineRenderer;
    GameObject _muzzle;
    GameObject _player;
    float timer;
    bool hit = false;

    private void Start()
    {
        _player = GameObject.Find("Player");
        _muzzle = GameObject.Find("AnchorMuzzle");
        transform.SetPositionAndRotation(_muzzle.transform.position, _muzzle.transform.rotation);
        _rig.velocity += transform.forward * 150f;
        timer = 0;
        _lineRenderer.material = _ropeMaterial;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 2 && !hit)
        {
            _player.GetComponent<PlayerMove>().NotHitAnchor();
        }
        _lineRenderer.SetPosition(0,_muzzle.transform.position);
        _lineRenderer.SetPosition(1,transform.position);
    }
    private void OnCollisionEnter(Collision collision)
    {
        _player.GetComponent <PlayerMove>().HitAnchor(transform.position);
        _rig.velocity = Vector3.zero;
        hit = true;
    }
}
