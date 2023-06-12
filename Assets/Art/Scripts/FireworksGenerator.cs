using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FireworksGenerator : EditorWindow
{
    private int m_Columns = 10;
    private float m_Length = 1f;
    private Texture2D m_ColorTex;
    private Texture2D m_PatternTex;

    private static Material m_Material;

    [MenuItem("Tools/Fireworks Generator")]
    static void Init()
    {
        FireworksGenerator window = GetWindow<FireworksGenerator>();
        window.Show();
    }

    void OnGUI()
    {
        m_Columns = EditorGUILayout.IntField("Columns", m_Columns);
        m_Length = EditorGUILayout.FloatField("Length", m_Length);
        m_ColorTex = EditorGUILayout.ObjectField("Color Texture", m_ColorTex, typeof(Texture2D), false) as Texture2D;
        m_PatternTex = EditorGUILayout.ObjectField("Pattern Texture", m_PatternTex, typeof(Texture2D), false) as Texture2D;

        if (GUILayout.Button("Generate"))
        {
            Generate();
        }
    }

    private void Generate()
    {
        if (m_ColorTex == null || m_PatternTex == null)
        {
            Debug.LogError("Color Texture or Pattern Texture is missing!");
            return;
        }

        // ����Mesh
        Mesh mesh = new Mesh();

        // ���ɶ���
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uv0 = new List<Vector2>();
        List<Vector2> uv1 = new List<Vector2>();
        List<Color> colors = new List<Color>();
        List<int> triangles = new List<int>();

        float halfLength = m_Length / 2f;
        Vector3 center = new Vector3(m_Columns * m_Length * 0.5f, 0f, m_Columns * m_Length * 0.5f);

        for (int x = 0; x < m_Columns; x++)
        {
            for (int y = 0; y < m_Columns; y++)
            {
                Vector3 startPos = new Vector3(x * m_Length, 0f, y * m_Length);

                // ����ƶ����ӵ�λ��
                float offsetX = Random.Range(-m_Length * 0.25f, m_Length * 0.25f);
                float offsetY = Random.Range(-m_Length * 0.25f, m_Length * 0.25f);
                startPos += new Vector3(offsetX, 0f, offsetY);

                // ���ɶ�������
                Vector3[] quadVertices = CalculateVertex(startPos, m_Length);
                vertices.AddRange(quadVertices);

                // ����UV0
                Vector2[] quadUV0 = CalculateUV0();
                uv0.AddRange(quadUV0);

                // ����UV1
                Vector2[] quadUV1 = CalculateUV1(startPos, halfLength);
                uv1.AddRange(quadUV1);

                // ���ɶ���ɫ
                Color color = SetPreVertexColor();
                colors.Add(color);
                colors.Add(color);
                colors.Add(color);
                colors.Add(color);

                // ��������������
                int[] quadTriangles = CalculateTriangle(x, y);
                triangles.AddRange(quadTriangles);
            }
        }

        // �ƶ����㵽���ĵ�
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] -= center;
        }

        // �����ɵ��������ø�Mesh
        mesh.SetVertices(vertices);
        mesh.SetUVs(0, uv0);
        mesh.SetUVs(1, uv1);
        mesh.SetColors(colors);
        mesh.SetTriangles(triangles, 0);

        // ����Mesh
        string savePath = EditorUtility.SaveFilePanel("Save Fireworks Mesh", "", "Fireworks", "asset");
        if (!string.IsNullOrEmpty(savePath))
        {
            savePath = FileUtil.GetProjectRelativePath(savePath);
            AssetDatabase.CreateAsset(mesh, savePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Fireworks Mesh saved at: " + savePath);
        }
    }

    private Vector3[] CalculateVertex(Vector3 startPos, float length)
    {
        Vector3[] vertices = new Vector3[4];

        // �����ĸ���������
        vertices[0] = startPos;
        vertices[1] = startPos + new Vector3(length, 0f, 0f);
        vertices[2] = startPos + new Vector3(0f, 0f, length);
        vertices[3] = startPos + new Vector3(length, 0f, length);

        return vertices;
    }

    private Vector2[] CalculateUV0()
    {
        Vector2[] uv0 = new Vector2[4];

        // ����UV0��ÿ�����Ӷ���Ϊ0-1
        uv0[0] = new Vector2(0f, 0f);
        uv0[1] = new Vector2(1f, 0f);
        uv0[2] = new Vector2(0f, 1f);
        uv0[3] = new Vector2(1f, 1f);

        return uv0;
    }

    private Vector2[] CalculateUV1(Vector3 startPos, float halfLength)
    {
        Vector2[] uv1 = new Vector2[4];

        // ����UV1��ÿ������ȡ���ĵ㣬�ڴ�����϶�Ӧ0-1��λ��
        Vector2 centerUV = new Vector2((startPos.x + halfLength) / (m_Columns * m_Length),
                                       (startPos.z + halfLength) / (m_Columns * m_Length));

        uv1[0] = centerUV;
        uv1[1] = centerUV;
        uv1[2] = centerUV;
        uv1[3] = centerUV;

        return uv1;
    }

    private Color SetPreVertexColor()
    {
        // ���������ɫ���������ö���ɫ��R��Gͨ��
        Color color = new Color(Random.value, Random.value, 0f, 1f);
        return color;
    }

    private int[] CalculateTriangle(int x, int y)
    {
        int[] triangles = new int[6];

        // ����ÿ�����ӵ���������������
        int vertexIndex = (x * m_Columns + y) * 4;
        triangles[0] = vertexIndex;
        triangles[1] = vertexIndex + 1;
        triangles[2] = vertexIndex + 2;
        triangles[3] = vertexIndex + 1;
        triangles[4] = vertexIndex + 3;
        triangles[5] = vertexIndex + 2;

        return triangles;
    }
}