using UnityEngine;

public class LookTowardCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    private CanvasGroup _canvasGroup;
    [SerializeField] private float _displayThreshold = .3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }


    // Update is called once per frame
    void Update()
    {
        var direction = -target.position + transform.position;
        _canvasGroup.alpha = direction.sqrMagnitude > _displayThreshold ? 1f : 0f;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
