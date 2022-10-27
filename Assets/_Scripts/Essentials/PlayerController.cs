using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side
{
    Left,
    Mid,
    Right
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    private AudioManager _getAudioManager;
    public Animator playerAnimator;
    private Side _whichSide = Side.Mid;
    private bool _swipeLeft, _swipeRight, _swipeUp, _isPlayerJumping;
    private const float XValue = 2;
    private const float StrafeSpeed = 1f;
    private const float SpeedMultiplier = 5f;
    private Vector3 _startPosition = Vector3.zero;
    private Vector3 _goalPosition = Vector3.zero;

    // Start is called before the first frame update
    private void Start()
    {
        _getAudioManager = AudioManager.Instance;
    }

    void Awake()
    {
        Instance = this;
        playerAnimator = GetComponent<Animator>();
        _startPosition = gameObject.transform.position;
        _startPosition.y = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Input settings
        _swipeLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        _swipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        _swipeUp = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        // Right and left movement
        if (_swipeLeft)
        {
            _getAudioManager.StrafeSounds = true;
            _getAudioManager.Sounds();
            if (_whichSide == Side.Mid)
            {
                playerAnimator.Play("strafeLeft");
                _goalPosition.x = -XValue;
                _whichSide = Side.Left;
            }
            else if (_whichSide == Side.Right)
            {
                playerAnimator.Play("strafeLeft");
                _goalPosition.x = 0;
                _whichSide = Side.Mid;
            }
        }

        if (_swipeRight)
        {
            _getAudioManager.StrafeSounds = true;
            _getAudioManager.Sounds();
            if (_whichSide == Side.Mid)
            {
                playerAnimator.Play("strafeRight");
                _goalPosition.x = XValue;
                _whichSide = Side.Right;
            }
            else if (_whichSide == Side.Left)
            {
                playerAnimator.Play("strafeRight");
                _goalPosition.x = 0;
                _whichSide = Side.Mid;
            }
        }

        // Jump movement
        if (_swipeUp && (_startPosition.y < 0.5f))
        {
            _isPlayerJumping = true;
            playerAnimator.Play("jump");
            _goalPosition.y = 6;
            _getAudioManager.JumpSounds = true;
            _getAudioManager.Sounds();
        }

        if (_startPosition.y > 3)
        {
            _goalPosition.y = 0;
            _isPlayerJumping = false;
        }

        // Make character move
        var lerpValue = StrafeSpeed * Time.deltaTime;
        transform.position = _isPlayerJumping
            ? Vector3.MoveTowards(_startPosition, _goalPosition, lerpValue * SpeedMultiplier)
            : Vector3.Lerp(_startPosition, _goalPosition, lerpValue * SpeedMultiplier);
        _startPosition = gameObject.transform.position;
    }

    public void DeathMove(bool isPlayerDead)
    {
        if (isPlayerDead)
        {
            _getAudioManager.DeathSounds = true;
            _getAudioManager.Sounds();
            playerAnimator.Play("death");
            ScreenShakeController.Instance.StartShake(1f, 0.5f);
        }
    }
}