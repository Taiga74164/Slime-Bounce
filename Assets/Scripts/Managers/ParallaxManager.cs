using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    // CODE ADAPTED FROM TUTORIAL: https://www.youtube.com/watch?v=zit45k6CUMk //

    private float _length;
    private float _startPos;

    public GameObject Camera;
    public float parallaxDepth;
    public bool loopingTexture = false;

    void Start()
    {
        _startPos = transform.position.y;
        _length = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        float temp = Camera.transform.position.y * (1 - parallaxDepth);
        float dist = Camera.transform.position.y * parallaxDepth;

        transform.position = new Vector3(transform.position.x, _startPos + dist, transform.position.z);

        if (loopingTexture)
        {
            if (temp > _startPos + _length)
                _startPos += _length;
            else if (temp < _startPos - _length)
                _startPos -= _length;
        }
    }
}
