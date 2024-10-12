using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    GameObject _muzzle;
    public float bulletSpeed = 1;
    float timer = 5;
    private void Start()
    {
        _muzzle = GameObject.Find("muzzle");
        transform.SetPositionAndRotation(_muzzle.transform.position, _muzzle.transform.rotation);
        _rb.velocity = transform.forward * bulletSpeed;
        timer = 5;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision != null)
        {
            Destroy(gameObject);
        }
    }
}
