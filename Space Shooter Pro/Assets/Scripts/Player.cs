using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldsActive = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private int _score;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;

    private UIManager _uiManager;

    private Coroutine _last_tripleShotPowerDownRoutine_enum;
    private Coroutine _last_SpeedBoostPowerDownRoutine_enum;

    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private AudioClip _laserSoundClip;
    
    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        
        // take the current position of game object and set it to (0,0,0)
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if(_spawnManager == null)
        {
            Debug.LogError("The sapwn manager is null");
        }
        if(_uiManager == null)
        {
            Debug.LogError("The UI manager is null");
        }
        if(_audioSource == null)
        {
            Debug.LogError("audio source on player is null");
        }
        

    }

    // Update is called once per frame
    // 60 frames are rendered per second
    // so 60 times this method is called per second
    void Update()
    {
        CalculateMovement();

        // if I hit the space key , spawn game object
        /*
        if ((Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire") )&& Time.time > _canFire)
        {
            FireLaser();
        }
        */

#if UNITY_ANDROID
        if (CrossPlatformInputManager.GetButtonDown("Fire") && Time.time > _canFire)
        {
            Debug.Log("android");
            //FireLaser();
        }
#endif

#if UNITY_EDITOR_OSX
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Debug.Log("editor");
            //FireLaser();
        }
#endif

    }

    void CalculateMovement()
    {
        // same as transform.Translate(new Vector3(1,0,0));
        // moves object one unit to right for each update call
        // Time.deltaTime converts frame rate manipulation to real time seconds manipulation
        // transform.Translate(Vector3.right * Time.deltaTime) -> this means...object is moved one unit to right per sec and not per frame .
        // transform.Translate(Vector3.right * 5 * Time.deltaTime) -> this means...object is moved 5 units to right per sec and not per frame .
        // Vector3.right is same as new Vector3(1,0,0)
        float horizontalInput, verticalInput;

        //horizontalInput = Input.GetAxis("Horizontal"));
        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);

        // verticalInput = Input.GetAxis("Vertical");
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        //transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime); same as before

        horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
        verticalInput = CrossPlatformInputManager.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);

        // bounding the player movement
        // if player position on the y is >=0 ,set y to 0
        // else if player position on the y is <= -3.3f then set y to -3.3f

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.3f)
        {
            transform.position = new Vector3(transform.position.x, -3.3f, 0);
        }

        //if player position on x is >=9.2 then set x to 9.2
        // else if player postion on x is <=-9.2 then set x to -9.2
        
        if (transform.position.x >= 9.2f)
        {
            transform.position = new Vector3(9.2f, transform.position.y, 0);
        }
        else if (transform.position.x <= -9.2f)
        {
            transform.position = new Vector3(-9.2f, transform.position.y, 0);
        }
        
        
        // The player bound along x-axis and y axis can be wrapped using the below approach also
        // transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.2f, 9.2f), transform.position.y, 0);
        // transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.3f, 0), 0);
    }

    public void FireLaser()
    {
        
        _canFire = Time.time + _fireRate;

        if(_isTripleShotActive == true)
        {
            // fire three lasers(triple_shot prefab)
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.02f, 0), Quaternion.identity);

        }


        // if tripleShotActive is true
        // fire three lasers(triple_shot prefab)
        // else
        // fire 1 laser

       
    }

    public void Damage()
    {
        // if shield is active
        // do nothing
        // deactivate shield
        // return

        if(_isShieldsActive == true)
        {
            _isShieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        switch(_lives)
        {
            case 1:
                StartCoroutine(SetEngineDamageAnim(_leftEngine));
                break;
            case 2:
                StartCoroutine(SetEngineDamageAnim(_rightEngine));
                break;
        }

        _uiManager.UpdateLives(_lives);

        if(_lives < 1)
        {
            // communicate with the spawn manager to stop enemy spawning
            _spawnManager.OnPlayerDeath();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        

        if (_isTripleShotActive)
        {
          
            StopCoroutine(_last_tripleShotPowerDownRoutine_enum);
        }
        _isTripleShotActive = true;
        // start the power Down Coroutine for triple shot
        _last_tripleShotPowerDownRoutine_enum = StartCoroutine(TripleShotPowerDownRoutine());
    }

    // IEnumerator TripleShotPowerDownRoutine
    // wait for 5 seconds
    // set the triple shot to false

    IEnumerator TripleShotPowerDownRoutine()
    {
       
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false; 
        
    }

    IEnumerator SetEngineDamageAnim(GameObject engine)
    {
        yield return new WaitForSeconds(0.5f);
        engine.SetActive(true);
    }

    public void SpeedBoostActive()
    {

        if(_isSpeedBoostActive)
        {
            StopCoroutine(_last_SpeedBoostPowerDownRoutine_enum);
        }
        _isSpeedBoostActive = true;
        _speed = 10f;
       _last_SpeedBoostPowerDownRoutine_enum =  StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {

        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed = 5f;
    }

    public void ShieldsActive()
    {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
    }

    // method to add the score
    // communicate with UI to update the score
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }


}
