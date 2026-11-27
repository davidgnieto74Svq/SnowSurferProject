using UnityEngine;

// Parallax background that follows the camera.
// Each layer is a GameObject with 3 copies of the same sprite side by side (left, center, right),
// so it can repeat forever horizontally.
// followX / followY: 1 = moves with the camera (looks infinitely far, like the sky),
//                    0 = stays still in the world (like the ground).
// Runs after Cinemachine has moved the camera, so the background doesn't shake.
[DefaultExecutionOrder(10000)]
public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class Layer
    {
        public Transform root;
        [Range(0f, 1f)] public float followX = 0.5f;
        [Range(0f, 1f)] public float followY = 0.9f;
        [HideInInspector] public Vector3 startPosition;
        [HideInInspector] public float tileWidth;
    }

    [SerializeField] private Layer[] layers;

    Transform cameraTransform;
    Vector3 cameraStartPosition;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        cameraStartPosition = cameraTransform.position;

        foreach (Layer layer in layers)
        {
            // Start every layer centered on the camera
            layer.root.position = new Vector3(cameraStartPosition.x, cameraStartPosition.y, layer.root.position.z);
            layer.startPosition = layer.root.position;

            SpriteRenderer sprite = layer.root.GetComponentInChildren<SpriteRenderer>();
            layer.tileWidth = sprite != null ? sprite.bounds.size.x : 0f;
        }
    }

    void LateUpdate()
    {
        Vector3 cameraMovement = cameraTransform.position - cameraStartPosition;

        foreach (Layer layer in layers)
        {
            float x = layer.startPosition.x + cameraMovement.x * layer.followX;
            float y = layer.startPosition.y + cameraMovement.y * layer.followY;

            // Jump one tile when the camera gets too far, so the 3 copies always cover the view
            if (layer.tileWidth > 0f)
            {
                float distance = cameraTransform.position.x - x;
                x += Mathf.Round(distance / layer.tileWidth) * layer.tileWidth;
            }

            layer.root.position = new Vector3(x, y, layer.root.position.z);
        }
    }
}
