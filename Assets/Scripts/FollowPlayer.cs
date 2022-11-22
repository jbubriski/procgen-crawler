using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Camera _camera;
    private Transform _player;
    private PlayerHealth _playerHealth;
    private PlayerMovement _playerMovement;

    private Vector3 _offset;
    private float _startingFov;
    private float _endingFov = 30;
    private float _fovZoomTimeMax = 2;
    private float _fovZoomTime = 0;

    public float MoveDampening = 4;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _playerMovement = FindObjectOfType<PlayerMovement>();

        _offset = transform.position;
        _startingFov = _camera.fieldOfView;
    }

    void Update()
    {
        if (_player != null)
        {
            if (_playerMovement != null && _playerMovement.Target != null)
            {
                // Move camera to center on area between player and target
                var distanceVector = _playerMovement.Target.transform.position - _player.position;
                var centerArea = _player.position + (distanceVector / 2);

                var targetPosition = centerArea + _offset;

                var toPosition = Vector3.Lerp(transform.position, targetPosition, MoveDampening * Time.deltaTime);

                transform.position = toPosition;
            }
            else
            {
                // Move the camera to center on the player
                var targetPosition = _player.position + _offset;

                var toPosition = Vector3.Lerp(transform.position, targetPosition, MoveDampening * Time.deltaTime);

                transform.position = toPosition;
            }
            
            if (_playerHealth.IsDead)
            {
                // Zoom to point of death
                _fovZoomTime += Time.deltaTime;

                var fov = Mathf.Lerp(_startingFov, _endingFov, _fovZoomTime / _fovZoomTimeMax);
                _camera.fieldOfView = fov;
            }
        }
    }

    public void SetPlayer(Transform player)
    {
        _player = player;

        if (player != null)
        {
            _playerHealth = player.GetComponent<PlayerHealth>();
            _playerMovement = FindObjectOfType<PlayerMovement>();
        }
    }

    public void Reset()
    {
        Debug.Log("Resetting fov");
        _camera.fieldOfView = _startingFov;
        _fovZoomTime = 0;
    }
}
