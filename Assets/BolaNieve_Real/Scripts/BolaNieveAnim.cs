using UnityEngine;

// Ponlo en el objeto BolaNieve (junto a BolaNieve.cs).
// Reproduce los 8 fotogramas en bucle, más rápido cuanto más rápida va la bola.
// IMPORTANTE: en BolaNieve.cs pon "Giro" = 0, porque ahora el giro lo dan los dibujos.
[RequireComponent(typeof(SpriteRenderer))]
public class BolaNieveAnim : MonoBehaviour
{
    public Sprite[] fotogramas;          // BolaNieve_1 ... BolaNieve_8, en orden
    public float fotogramasPorSegundo = 14f;

    SpriteRenderer sr;
    float t;

    void Awake() => sr = GetComponent<SpriteRenderer>();

    void Update()
    {
        if (fotogramas == null || fotogramas.Length == 0) return;
        t += Time.deltaTime * fotogramasPorSegundo;
        sr.sprite = fotogramas[(int)t % fotogramas.Length];
    }
}
