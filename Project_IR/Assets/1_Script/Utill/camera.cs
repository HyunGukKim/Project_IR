using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class camera : MonoBehaviour {
    [SerializeField] private Camera _camera;

    [Header("기준 화면 비율")]
    public Vector2 referenceSize = new Vector2(1080 , 1920);
    private Vector2 screenSize = new Vector2(Screen.width, Screen.height);

    [Header("매 프레임 업데이트 여부")]
    public bool UpdateEveryFrame = false;

    private void Start() {
        Set();
    }

    private void Update() {
        if (UpdateEveryFrame)
            Set();
    }

    private void Set() {
        screenSize = new Vector2(Screen.width, Screen.height);
        float baseOrthographicSize = (referenceSize.y / 100f) / 2f; // Camera의 OrthographicSize는 카메라 y축 크기의 절반이므로 이렇게 구함.

        float screenRatio = screenSize.y / screenSize.x;
        float referenceRatio = referenceSize.y / referenceSize.x;

        if (screenRatio > referenceRatio) {
            var sizeMultiply = screenRatio / referenceRatio;
            _camera.orthographicSize = baseOrthographicSize * sizeMultiply;
        } else {
            _camera.orthographicSize = baseOrthographicSize;
        }
    }
}
