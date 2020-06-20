using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    // ID for powerups
    // 0 - Triple shots
    // 1 - Speed
    // 2 - shield
    [SerializeField]
    private int powerupID;

    
    private AudioSource _audioSource;
    

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("AudioSource source on powerup is null");
        }
    }

    // Update is called once per frame
    void Update()
    {

        // move down at a speed of 3 meters per second (adjust in inspector)
        // when we leave the screen destroy this object

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y <= -5f)
        {
            Destroy(gameObject);
        }

    }

    // OnTriggerCollision
    // Only be collectible by the player (Hint : Use Tag)
    // On collected destroy this object

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            // communicates with the player script
            Player player = other.transform.GetComponent<Player>();
            
            if(player != null)
            {
                _audioSource.Play();
                switch(powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;

                    case 1:
                        player.SpeedBoostActive();
                        break;

                    case 2:
                        player.ShieldsActive();
                        break;

                    default:
                        Debug.Log("Default value");
                        break;

                }
            }
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            _speed = 0;
            Destroy(gameObject,1f);
        }
    }
}
