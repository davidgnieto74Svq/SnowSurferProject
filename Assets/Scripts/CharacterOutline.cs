using UnityEngine;

// Accessibility: draws a dark outline around the character so it stands out on the white snow.
// It creates 4 dark copies of the sprite (up, down, left, right) behind it,
// and copies the current animation frame every frame.
[RequireComponent(typeof(SpriteRenderer))]
public class CharacterOutline : MonoBehaviour
{
    [SerializeField] private Color outlineColor = new Color(0.02f, 0.05f, 0.15f, 1f);
    [SerializeField] private float thickness = 0.1f; // In world units

    SpriteRenderer mainRenderer;
    SpriteRenderer[] copies;

    void Start()
    {
        mainRenderer = GetComponent<SpriteRenderer>();
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        copies = new SpriteRenderer[directions.Length];

        for (int i = 0; i < directions.Length; i++)
        {
            GameObject copy = new GameObject("Outline");
            copy.transform.SetParent(transform, false);
            // Convert the world thickness to local units (the character can be scaled)
            Vector3 scale = transform.lossyScale;
            copy.transform.localPosition = new Vector3(directions[i].x * thickness / scale.x, directions[i].y * thickness / scale.y, 0f);

            SpriteRenderer renderer = copy.AddComponent<SpriteRenderer>();
            renderer.color = outlineColor;
            renderer.sortingLayerID = mainRenderer.sortingLayerID;
            renderer.sortingOrder = mainRenderer.sortingOrder - 1;
            copies[i] = renderer;
        }
    }

    void LateUpdate()
    {
        foreach (SpriteRenderer copy in copies)
        {
            copy.sprite = mainRenderer.sprite;
            copy.flipX = mainRenderer.flipX;
        }
    }
}
