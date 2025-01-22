using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    [SerializeField] protected RectTransform rt = default;

    Rect safeArea;
    protected virtual void Awake() {
        Init();
    }

    private void Start() {
        Init();
    }

    protected virtual void Init() {
        safeArea = Screen.safeArea;
        Vector2 minAnchor = safeArea.position;
        Vector2 maxAnchor = minAnchor + safeArea.size;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        rt.anchorMin = minAnchor;
        rt.anchorMax = maxAnchor;
    }
}
