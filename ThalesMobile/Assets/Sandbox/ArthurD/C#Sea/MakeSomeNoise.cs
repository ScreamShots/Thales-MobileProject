using UnityEngine;

public class MakeSomeNoise : MonoBehaviour
{
    public float power = 3;
    public float scale = 1;
    public float timeScale = 1;

    public float offSetX;
    public float offSetY;
    public MeshFilter mf;

    private void Start()
    {
        MakeNoise();
    }

    void Update()
    {
        MakeNoise();
        offSetX += Time.deltaTime * timeScale;
        offSetY += Time.deltaTime * timeScale;
    }

    private void MakeNoise()
    {
        Vector3[] verticies = mf.mesh.vertices;

        for (int i = 0; i < verticies.Length; i++)
        {
            verticies[i].y = CalculateHeight(verticies[i].x, verticies[i].z) * power;
        }

        mf.mesh.vertices = verticies;
    }

    private float CalculateHeight(float x, float y)
    {
        float xCoord = x * scale + offSetX;
        float yCoord = y * scale + offSetY;

        return Mathf.PerlinNoise(xCoord,yCoord);
    }
}
