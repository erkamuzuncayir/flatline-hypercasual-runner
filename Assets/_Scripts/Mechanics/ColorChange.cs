using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class ColorChange : MonoBehaviour
{
    public static ColorChange Instance;
    private AudioManager _getAudioManager;
    private Renderer _playerColorRenderer;
    private Color _playerColor;
    public string collidedObjectTag, collidedObjectName, collidedObjectCollectable;
    [FormerlySerializedAs("DeathStatus")] public bool deathStatus;
    private float _playerXPos;
    private int _multiplierTargetScore = 50;
    private float _difficultyMultiplier = 1.2f;
    private float _colorChangeValue = 1;
    public int scoreInColorChange;
    [SerializeField] private string canPassObstacle;

    private void Start()
    {
        _getAudioManager = AudioManager.Instance;
        scoreInColorChange = GameManager.Instance.score;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        _playerColorRenderer = gameObject.GetComponentInChildren<Renderer>();
        var sharedMaterial = _playerColorRenderer.sharedMaterial;
        _playerColor = sharedMaterial.color;
        sharedMaterial.color = new Color(1, 1, 1, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        var triggeredObject = other.gameObject;
        collidedObjectTag = triggeredObject.tag;
        collidedObjectName = triggeredObject.name;
        switch (collidedObjectTag)
        {
            case "Obstacle" when canPassObstacle != collidedObjectName:
                deathStatus = true;
                PlayerController.Instance.DeathMove(deathStatus);
                GameManager.Instance.IsPlayerDead = true;
                _getAudioManager.DeathSounds = true;
                break;
            case "Obstacle" when canPassObstacle == collidedObjectName:
                scoreInColorChange += 5;
                triggeredObject.SetActive(false);
                break;
        }

        PointCalculator(triggeredObject);
    }

    // Update is called once per frame
    private void Update()
    {
        _playerXPos = this.transform.position.x;
        if (_playerColor.r is < 0 or > 1)
        {
            _playerColor.r = Mathf.Round(_playerColor.r);
        }

        if (_playerColor.g is < 0 or > 1)
        {
            _playerColor.g = Mathf.Round(_playerColor.g);
        }

        if (_playerColor.b is < 0 or > 1)
        {
            _playerColor.b = Mathf.Round(_playerColor.b);
        }

        ColorChanger();
    }

    private void ColorChanger()
    {
        if (collidedObjectTag is not "GreenRoad" or "BlueRoad" && _playerXPos < -1)
        {
            if (_playerColor.r is >= -0.1f and <= 1.1f && _playerColor.g is >= -0.1f and <= 1.1f &&
                _playerColor.b is >= -0.1f and <= 1.1f)
            {
                _playerColor.r += (Time.deltaTime * _colorChangeValue);
                _playerColor.g -= (Time.deltaTime * _colorChangeValue);
                _playerColor.b -= (Time.deltaTime * _colorChangeValue);
                _playerColorRenderer.sharedMaterial.color = _playerColor;
            }

            canPassObstacle = _playerColor.r > 0.7f ? "RedObstacle(Clone)" : "cannot";
        }
        else if (collidedObjectTag is not "RedRoad" or "BlueRoad" && _playerXPos is < 1 and > -1)
        {
            if (_playerColor.r is >= -0.1f and <= 1.1f && _playerColor.g is >= -0.1f and <= 1.1f &&
                _playerColor.b is >= -0.1f and <= 1.1f)
            {
                _playerColor.r -= (Time.deltaTime * _colorChangeValue);
                _playerColor.g += (Time.deltaTime * _colorChangeValue);
                _playerColor.b -= (Time.deltaTime * _colorChangeValue);
                _playerColorRenderer.sharedMaterial.color = _playerColor;
            }

            canPassObstacle = _playerColor.g > 0.7f ? "GreenObstacle(Clone)" : "cannot";
        }
        else if (collidedObjectTag is not "RedRoad" or "GreenRoad" && _playerXPos > 1)
        {
            if (_playerColor.r is >= -0.1f and <= 1.1f && _playerColor.g is >= -0.1f and <= 1.1f &&
                _playerColor.b is >= -0.1f and <= 1.1f)
            {
                _playerColor.r -= (Time.deltaTime * _colorChangeValue);
                _playerColor.g -= (Time.deltaTime * _colorChangeValue);
                _playerColor.b += (Time.deltaTime * _colorChangeValue);
                _playerColorRenderer.sharedMaterial.color = _playerColor;
            }

            canPassObstacle = _playerColor.b > 0.7f ? "BlueObstacle(Clone)" : "cannot";
        }
    }

    private void PointCalculator(GameObject other)
    {
        var triggeredCollectable = other.name;
        switch (triggeredCollectable)
        {
            case "FivePoints(Clone)":
                _getAudioManager.CollectSounds = true;
                _getAudioManager.Sounds();
                scoreInColorChange += 5;
                other.gameObject.SetActive(false);
                break;
            case "TenPoints(Clone)":
                _getAudioManager.CollectSounds = true;
                _getAudioManager.Sounds();
                scoreInColorChange += 10;
                other.gameObject.SetActive(false);
                break;
            case "TwentyPoints(Clone)":
                _getAudioManager.CollectSounds = true;
                _getAudioManager.Sounds();
                scoreInColorChange += 20;
                other.gameObject.SetActive(false);
                break;
            case "FiftyPoints(Clone)":
                _getAudioManager.CollectSounds = true;
                _getAudioManager.Sounds();
                scoreInColorChange += 50;
                other.gameObject.SetActive(false);
                break;
        }

        GameManager.Instance.scoreUI.text = "Score: " + scoreInColorChange;
        if (scoreInColorChange < _multiplierTargetScore) return;
        StartCoroutine(GameManager.Instance.DifficultyWarning("Difficulty Increased!", 2));
        SpawnManager.Instance.gameSpeed *= _difficultyMultiplier;
        _colorChangeValue *= _difficultyMultiplier;
        Road.Instance.colorChangeValue *= _difficultyMultiplier;
        _multiplierTargetScore = (_multiplierTargetScore * 2);
    }
}