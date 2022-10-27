using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{
    public static ScreenShakeController Instance;
    private float _shakeTimeRemaining, _shakePower, _shakeFadeTime;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Player.playerInstance.isPlayerDead)
        //{
        //    StartShake(.5f, 1f);
        //}
    }
    private void LateUpdate()
    {
        if (_shakeTimeRemaining > 0)
        {
            _shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-0.25f, 0.25f) * _shakePower;
            float yAmount = Random.Range(-0.25f, 0.25f) * _shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0f);

            _shakePower = Mathf.MoveTowards(_shakePower, 0f, _shakeFadeTime * Time.deltaTime);
        }
    }
    public void StartShake(float length, float power)
    {
        _shakeTimeRemaining = length;
        _shakePower = power;

        _shakeFadeTime = power / length;
    }
}
