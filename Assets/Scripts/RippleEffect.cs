using UnityEngine;
using System.Collections.Generic;

public class RippleEffect : MonoBehaviour
{
    public float rippleSpeed = 5.0f;
    public float rippleDecay = 0.95f;
    public float rippleMaxIntensity = 0.1f;
    public float rippleSize = 1.0f;

    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;
    private List<Ripple> ripples = new List<Ripple>();

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
    }

    void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    ripples.Add(new Ripple(hit.point, rippleMaxIntensity, rippleDecay, rippleSpeed, rippleSize));
                }
            }
        }

        // Update ripples and the mesh
        UpdateRipples();
    }

    void UpdateRipples()
    {
        // Reset displaced vertices to the original
        originalVertices.CopyTo(displacedVertices, 0);

        // Update each ripple and apply its effect to the mesh
        for (int i = ripples.Count - 1; i >= 0; i--)
        {
            Ripple ripple = ripples[i];
            if (ripple.UpdateRipple(Time.deltaTime))
            {
                ApplyRippleEffect(ripple);
            }
            else
            {
                ripples.RemoveAt(i);
            }
        }

        // Update the mesh with displaced vertices
        mesh.vertices = displacedVertices;
        mesh.RecalculateNormals();
    }

    void ApplyRippleEffect(Ripple ripple)
    {
        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(originalVertices[i]);
            float distance = Vector3.Distance(worldPos, ripple.Center) / ripple.Size;
            float rippleEffect = Mathf.Sin(distance - ripple.Time) * ripple.Intensity / (1.0f + distance);
            displacedVertices[i] += Vector3.up * rippleEffect;
        }
    }

    private class Ripple
    {
        public Vector3 Center { get; private set; }
        public float Intensity { get; private set; }
        public float Decay { get; private set; }
        public float Speed { get; private set; }
        public float Size { get; private set; }
        public float Time { get; private set; }

        public Ripple(Vector3 center, float intensity, float decay, float speed, float size)
        {
            Center = center;
            Intensity = intensity;
            Decay = decay;
            Speed = speed;
            Size = size;
            Time = 0;
        }

        public bool UpdateRipple(float deltaTime)
        {
            Time += deltaTime * Speed;
            Intensity *= Decay;
            return Intensity > 0.001f;
        }
    }
}
