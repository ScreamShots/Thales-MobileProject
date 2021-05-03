using UnityEngine;

public class MakeSomeNoise : MonoBehaviour
{
    [Header("Component")]
    public MeshFilter mf;
    private Vector3[] verticies;

    [Header("Variable")]
    public float timeScale = 0.1f;
    [Space(10)]
    public float scale = 1;
    public float offsetSin = 0;

    [SerializeField] private float offSetX;
    [SerializeField] private float offSetY;


    private void Start()
    {
        verticies = mf.mesh.vertices;
        for (int i = 0; i < verticies.Length; i++)
        {
            verticies[i].y = CalculateHeight(verticies[i].x, verticies[i].z);
        }

        mf.mesh.vertices = verticies;
    }

    void Update()
    {
        ApplyNoiseToVertice();
        offSetX += timeScale * Time.fixedDeltaTime;
        offSetY += timeScale * Time.fixedDeltaTime;
    }

    [ContextMenu("Preview Result")]
    private void ApplyNoiseToVertice()
    {
        verticies = mf.mesh.vertices;

        for (int i = 0; i < verticies.Length; i++)
        {
            verticies[i].y = CalculateHeight(verticies[i].x, verticies[i].z);
        }

        mf.mesh.vertices = verticies;
    }

    private float CalculateHeight(float x, float y)
    {
        /* Virgin Perlin Noise
        
        float xCoord = x * scale + offSetX;
        float yCoord = y * scale + offSetY;

        return Mathf.PerlinNoise(xCoord,yCoord);
        */

        float result = x + (offSetX /*% Mathf.PI*/);
        result *= scale * Mathf.PI;
        result %= Mathf.PI;
        result += offsetSin * Mathf.PI;
        result = Mathf.Abs(result);
        result = Mathf.Cos(result);

        return result;
    }
}
