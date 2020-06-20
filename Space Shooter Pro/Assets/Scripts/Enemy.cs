using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;

    private Animator _anim;

    private BoxCollider2D _boxCollider;

    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _explosionAudioClip;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _boxCollider = GetComponent<BoxCollider2D>();

        if (_player == null)
        {
            Debug.LogError("Player is null");
        }
        _anim = GetComponent<Animator>();

        if(_anim == null)
        {
            Debug.LogError("Animator is null");
        }

        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("audio clip on enemey explosion is null");
        }
        else
        {
            _audioSource.clip = _explosionAudioClip;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // move down at 4 meters per second

        // if it reached bottom of screen, respawn at top with random x position

        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        /*
        if(transform.position.y < -5f)
        {
            float randomX = Random.Range(-9.2f, 9.2f);
            transform.position = new Vector3(randomX, 7, 0);

        }
        */
 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if other is player
        // damage player(ie reduce player lives)
        // destroy enemy

        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)  // for better error handling
            {
                player.Damage();
            }
            _speed = 0;
            _boxCollider.enabled = false;
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2.8f);
        }

        // if other is laser
        // destroy laser
        // destroy enemy

        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddScore(10);
            }
            _speed = 0;
            _boxCollider.enabled = false;
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(this.gameObject, 2.8f);
        }

        _audioSource.Play();


        
    }
}
