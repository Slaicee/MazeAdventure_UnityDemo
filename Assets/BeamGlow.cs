using UnityEngine;

public class BeamGlow : MonoBehaviour
{
    public Renderer rend;
    public float glowSpeed = 2f;
    public Color baseColor = Color.cyan;

    void Start()
    {
        if (rend == null) rend = GetComponent<Renderer>();
    }

    void Update()
    {
        float emission = Mathf.PingPong(Time.time * glowSpeed, 1.0f);
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission + 0.3f);
        rend.material.SetColor("_EmissionColor", finalColor);
    }
}
