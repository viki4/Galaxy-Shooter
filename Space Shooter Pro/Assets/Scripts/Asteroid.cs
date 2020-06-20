using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 15.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private float _speed = 1.0f;
    private float _degrees_to_rotate = 45f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
    }

    // check for laser collision (trigger)
    // instantiate explosion at asteroid position
    // destroy the explosion after 3 seconds

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser" || other.tag == "Enemy")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject, 0.2f);
        }
        else if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)  // for better error handling
            {
                player.Damage();
            }
            _speed = 0;
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject, 0.2f);

        }
        
    }
}
