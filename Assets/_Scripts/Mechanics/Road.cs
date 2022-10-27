using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Road : MonoBehaviour
{
    public static Road Instance;
    private Renderer _roadColorRenderer;
    private Color _roadColor;
    public float colorChangeValue, difficultyMultiplier = 1;
    private bool _increase = true;

    // Start is called before the first frame update
    void Start()
    {
        _roadColorRenderer = gameObject.GetComponent<Renderer>();
        var sharedMaterial = _roadColorRenderer.sharedMaterial;
        _roadColor = sharedMaterial.color;
        sharedMaterial.color = new Color(1,1,1,1);
    }

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        colorChangeValue = Time.deltaTime * difficultyMultiplier;
        if (_roadColor.a is < 0 or > 1)
        {
            _roadColor.a = Mathf.Round(_roadColor.a);
        }

        switch (_increase)
        {
            case true when _roadColor.a is >= -0.1f and <= 1.1f:
                _roadColor.a += colorChangeValue;
                _roadColorRenderer.sharedMaterial.color = _roadColor;
                break;
            case false when _roadColor.a is >= -0.1f and <= 1.1f:
                _roadColor.a -= colorChangeValue;
                _roadColorRenderer.sharedMaterial.color = _roadColor;
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _increase = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _increase = true;
        }
    }
}