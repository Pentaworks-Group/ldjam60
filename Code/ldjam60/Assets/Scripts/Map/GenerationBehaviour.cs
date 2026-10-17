using Assets.Scripts.Core.Definitions;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityVector2 = UnityEngine.Vector2;
using UnityVector3 = UnityEngine.Vector3;
using Vector2 = GameFrame.Core.Math.Vector2;


namespace Assets.Scripts.Map
{
    public class GenerationBehaviour : MonoBehaviour
    {




        public void Start()
        {

            Debug.Log("Starting");
            Vector2 size = new(1000, 1000);
            MapDefinition _definition = new MapDefinition()
            {
                Size = size,
                TextureMap = GenerateTestEnvironment(1, size)
            };


            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
            MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
            Mesh mesh = new Mesh();


            int xSize = _definition.TextureMap.GetLength(0);
            int ySize = _definition.TextureMap.GetLength(1);

            Debug.Log(_definition.TextureMap);

            var boundaryPoints = GetBoundingPoints(_definition, 1);


            int numBoundaryPoints = boundaryPoints.Count;



            UnityVector2[] uvs = new UnityVector2[numBoundaryPoints];
            UnityVector3[] vertices = new UnityVector3[numBoundaryPoints];
            for (int i = 0; i < numBoundaryPoints; i++)
            {
                Vector2 point = boundaryPoints[i];
                float flippedY = ySize - point.Y;

                vertices[i] = new Vector3(point.X, flippedY, 0f);

                float u = point.X / xSize;
                float v = flippedY / ySize;

            }

            mesh.vertices = vertices;


            List<int> triangles = GetTriangleList(boundaryPoints);
            triangles.Reverse();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();


            mesh.uv = uvs;

            meshFilter.mesh = mesh;

            MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();

            this.gameObject.transform.position = new UnityVector3(-xSize / 2, -ySize / 2, -10);
        }

        private static List<int> GetTriangleList(List<Vector2> boundingPoints)
        {
            List<int> indexList = new List<int>();
            for (int i = 0; i < boundingPoints.Count; i++)
            {
                indexList.Add(i);
            }
            List<int> triangleList = new List<int>();



            Debug.Log($"GetTriangleList: {indexList.Count}");

            for (int i = indexList.Count - 1; i >= 0; i--)
            {
                int prev = indexList[WrapIndex(i - 1, indexList.Count)];
                int curr = indexList[i];
                int next = indexList[WrapIndex(i + 1, indexList.Count)];

                Vector2 p = boundingPoints[prev];
                Vector2 c = boundingPoints[curr];
                Vector2 n = boundingPoints[next];

                if (System.Math.Abs(Vector2.CrossProduct(c - p, n - p)) < 0.001f)
                {
                    indexList.RemoveAt(i);
                }
            }

            while (indexList.Count > 2)
            {
                bool earFound = false;
                for (int i = 0; i < indexList.Count; i++)
                {
                    int prev = indexList[WrapIndex(i - 1, indexList.Count)];
                    int curr = indexList[i];
                    int next = indexList[WrapIndex(i + 1, indexList.Count)];

                    if (IsEar(prev, curr, next, boundingPoints, indexList))
                    {
                        triangleList.Add(prev);
                        triangleList.Add(curr);
                        triangleList.Add(next);

                        indexList.RemoveAt(i);
                        earFound = true;
                        break;
                    }
                }

                if (!earFound)
                {
                    Debug.LogError($"Triangulation failed. Ensure boundary vertices are strictly clockwise/counter-clockwise without self-intersections. {triangleList.Count}, {indexList.Count}");
                    break;
                }

            }

            return triangleList;
        }

        private static bool IsEar(int pIdx, int cIdx, int nIdx, List<Vector2> points, List<int> indexList)
        {
            Vector2 p = points[pIdx];
            Vector2 c = points[cIdx];
            Vector2 n = points[nIdx];

            float crossProduct = Vector2.CrossProduct(c - p, n - p);
            if (crossProduct >= 0.0001f)
            {
                return false;
            }

            for (int i = 0; i < indexList.Count; i++)
            {
                int idx = indexList[i];
                if (idx == pIdx || idx == cIdx || idx == nIdx) continue;

                if (IsPointInTriangle(points[idx], p, c, n))
                {
                    return false;
                }
            }

            return true;
        }

        private static List<Vector2> GetBoundingPoints(MapDefinition mapDefinition, int material)
        {
            List<Vector2> leftSide = new();
            List<Vector2> rightSide = new();

            var texture = mapDefinition.TextureMap;
            int numRows = texture.GetLength(0);
            int numCols = texture.GetLength(1);


            for (int r = 0; r < numRows; r++)
            {
                int firstOneIndex = -1;
                int lastOneIndex = -1;

                for (int c = 0; c < numCols; c++)
                {
                    if (texture[r, c] == material)
                    {
                        if (firstOneIndex == -1)
                        {
                            firstOneIndex = c;
                        }
                        lastOneIndex = c;
                    }
                }

                if (firstOneIndex != -1)
                {
                    leftSide.Add(new Vector2(firstOneIndex, r));
                    if (lastOneIndex != -1)
                    {
                        rightSide.Add(new Vector2(lastOneIndex, r));
                    }
                }
            }
            List<Vector2> bountdingVertices = new(leftSide);
            for (int i = rightSide.Count - 1; i >= 0; i--)
            {
                bountdingVertices.Add(rightSide[i]);
            }
            return bountdingVertices;
        }


        private static bool IsPointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            float d1 = Vector2.CrossProduct(pt - v1, v2 - v1);
            float d2 = Vector2.CrossProduct(pt - v2, v3 - v2);
            float d3 = Vector2.CrossProduct(pt - v3, v1 - v3);

            bool has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            bool has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(has_neg && has_pos);
        }


        private static int[,] GenerateTestEnvironment(int material, Vector2 size)
        {
            int width = (int)size.X;
            int height = (int)size.Y;

            int[,] texture = new int[width, height];

            // Random seed: each call produces a different landscape
            float seed = Random.Range(0f, 1000f);

            for (int c = 0; c < width; c++)
            {
                // Layered Perlin noise: broad hills + medium bumps + fine detail
                float noise = Mathf.PerlinNoise((c + seed) * 0.05f, seed) * 0.5f
                            + Mathf.PerlinNoise((c + seed) * 0.15f, seed + 100f) * 0.3f
                            + Mathf.PerlinNoise((c + seed) * 0.5f, seed + 200f) * 0.2f;

                float columnHeight = noise * height * 0.45f;
                columnHeight = Mathf.Clamp(columnHeight, 1, height - 1);

                // Fill from the bottom up: ground sits at the bottom of the texture
                for (int r = height - 1; r >= height - columnHeight; r--)
                {
                    texture[r, c] = material;
                }
            }
            return texture;
        }

        private static int WrapIndex(int index, int count)
        {
            return (index % count + count) % count;
        }

    }
}
