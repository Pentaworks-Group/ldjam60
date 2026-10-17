using Assets.Scripts.Core.Definitions;
using UnityEngine;
using Vector2 = GameFrame.Core.Math.Vector2;
using UnityVector3 = UnityEngine.Vector3;
using UnityVector2 = UnityEngine.Vector2;
using System.Collections.Generic;


namespace Assets.Scripts.Map
{
    public class GenerationBehaviour : MonoBehaviour
    {
        private MapDefinition _definition = new MapDefinition()
        {
            Size = new Vector2(100, 100),
            TextureMap = GenerateTriangleTexture()
        };


        public void Start()
        {

            Debug.Log("Starting");
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

        private static int[,] GenerateTriangleTexture()
        {
            Debug.Log("Generating texture");
            int[,] texture = new int[100, 100];

            // Define the vertical bounds of the triangle
            int topRow = 10;
            int bottomRow = 90;
            int centerCol = 50;

            for (int row = topRow; row <= bottomRow; row++)
            {
                // Linearly scale the half-width of the triangle from 0 at the peak 
                // to 40 at the baseline (row 90)
                double progress = (double)(row - topRow) / (bottomRow - topRow);
                int halfWidth = (int)(progress * 40);

                int startCol = centerCol - halfWidth;
                int endCol = centerCol + halfWidth;

                // Fill the span for the current row
                for (int col = startCol; col <= endCol; col++)
                {
                    texture[row, col] = 1;
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
