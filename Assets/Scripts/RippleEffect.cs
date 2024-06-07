using Unity.Jobs;
using UnityEngine;

public class RippleEffect : MonoBehaviour
{
    public Material RippleMaterial;
    public Texture2D RippleTexture;

    float[][] rippleN, rippleNM1, rippleNP2;
    float lx = 10;//width
    float ly = 10;//height

    [SerializeField] float densityX = 0.1f; // x-axis density

    float densityY { get { return densityX; } } // y-axis density
    int Resx, Resy; // resolution

    public float Offset = 0.5f;
    public float c = 10;
    public float TimeStep;

    float currentTime;
    private void Start()
    {
        Resx = Mathf.FloorToInt(lx / densityX);
        Resy = Mathf.FloorToInt(ly / densityY);
        RippleTexture = new Texture2D(Resx, Resy, TextureFormat.RGBA32, false);

        rippleN = new float[Resx][];
        rippleNM1 = new float[Resx][];
        rippleNP2 = new float[Resx][];
        for (int i = 0; i < Resx; i++)
        {
            rippleN[i] = new float[Resy];
            rippleNM1[i] = new float[Resy];
            rippleNP2[i] = new float[Resy];
        }

        RippleMaterial.SetTexture("_MainTex", RippleTexture);
        RippleMaterial.SetTexture("_Displacement", RippleTexture);
    }
    private void Update()
    {
        RippleStep();
        ApplyMatrixToTexture(rippleN, ref RippleTexture,2);
    }
    void RippleStep()
    {
        TimeStep = Offset * densityX / c;
        currentTime += densityX;

        for (int i = 0; i < Resx; i++)
        {
            for (int j = 0; j < Resy; j++)
            {
                rippleNM1[i][j] = rippleN[i][j];
                rippleN[i][j] = rippleNP2[i][j];
            }
        }
        rippleN[50][50]=TimeStep*TimeStep*20*Mathf.Sin(currentTime*Mathf.Rad2Deg);
        for (int i = 1; i < Resx - 1; i++)
        {
            for (int j = 1; j < Resy - 1; j++)
            {
                float x = rippleN[i][j];
                float y = rippleN[i + 1][j];
                float z = rippleN[i - 1][j];
                float x1 = rippleN[i][j + 1];
                float y1 = rippleN[i][j - 1];
                float z1 = rippleNM1[i][j];
                rippleNP2[i][j] = 2f * x - z1 + Offset * Offset * (y1 + x1 + z + y - 4f * x);// ripple equation ;
            }
        }
    }

    void ApplyMatrixToTexture(float[][] state, ref Texture2D texture,float colorMultiplier)
    {
        for (int i = 0; i < Resx; i++)
        {
            for (int j = 0; j < Resy; j++)
            {
                float val = state[i][j]*colorMultiplier;
                texture.SetPixel(i, j, new Color(val + 0.5f, val + 0.5f, val + 0.5f, 1f));
            }
        }
        texture.Apply();
    }

}
