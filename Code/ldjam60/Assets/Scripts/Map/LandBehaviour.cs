using Assets.Scripts.Core.Definitions;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityVector3 = UnityEngine.Vector3;
using Vector2 = GameFrame.Core.Math.Vector2;
using Vector2Int = GameFrame.Core.Math.Vector2Int;

namespace Assets.Scripts.Map
{
    public class LandBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Material Ground;

        private MapDefinition currentMap;
        private List<Vector2> boundaryPoints;

        private float relativeMapWidht;
        private float relativeMapHeight;

        private void Awake()
        {
            //Base.Core.Game.ExecuteAfterInstantation(Init);
            Init();
        }


        public float CreateFlatSurface(float centerCoordinate, float width)
        {

            int centerX = GetIndexFromCoordinateX(centerCoordinate);
            int half = Mathf.CeilToInt((width * relativeMapWidht) / 2);
            int start = Mathf.Max(0, centerX - half);
            int end = Mathf.Min(currentMap.Size.X - 1, centerX + half);

            var texture = currentMap.TextureMap;

            int height = currentMap.Size.Y;
            for (int y = height - 1; y >= 0; y--)
            {
                if (texture[centerX, y] != 0)
                {
                    height = y;
                    break;
                }
            }


            var material = texture[centerX, height];

            for (int i = start; i <= end; i++)
            {
                if (texture[i, height] != 0)
                {
                    int y = height + 1;
                    while (y < currentMap.Size.Y && texture[i, y] != 0)
                    {
                        texture[i, y] = 0;
                        y++;
                    }
                }
                else
                {
                    int y = height;
                    while (y > 0 && texture[i, y] == 0)
                    {
                        texture[i, y] = material;
                        y--;
                    }
                }
            }

            RebuildMesh();

            return GetCoordinateFromIndexY(height);
        }

        private int GetIndexFromCoordinateX(float coordinate)
        {            
            var start = transform.position.x;
            var diff = coordinate - start;
            var rawIndex = diff / relativeMapWidht;
            return (int)rawIndex;
        }


        private float GetCoordinateFromIndexY(float index)
        {
            var y = transform.position.y;
            var relativeY = index * relativeMapHeight;
            var finalY = y + relativeY;
            Debug.Log($"y: {y}, relativeY:{relativeY}, finalY:{finalY}, relativeMapHeight:{relativeMapHeight}");
            return finalY;
        }

        private void Init()
        {
            Vector2Int size = new(1000, 1000);

            currentMap = new MapDefinition()
            {
                Size = size,
                TextureMap = GenerateTestEnvironment(1, size)
            };

            //ExportToPNG(map.TextureMap, "environmentPng.png");

            Physics.gravity = new UnityVector3(0, 3 * Physics.gravity.y, 0);



            int xSize = currentMap.Size.X;
            int ySize = currentMap.Size.Y;
            this.gameObject.transform.position = new UnityVector3(-xSize / 2, -ySize / 2, 0);

            RebuildMesh();

            var renderer = GetComponent<Renderer>();
            var sizeX = renderer.bounds.size.x;
            this.relativeMapWidht = currentMap.Size.X / sizeX;
            Debug.Log($"sizeX: {sizeX}, currentMap.Size.X:{currentMap.Size.X}, relativeMapWidht:{relativeMapWidht}");


            this.relativeMapHeight = (currentMap.Size.Y / currentMap.Size.X) * relativeMapWidht;

            Debug.Log($"currentMap.Size.Y:{currentMap.Size.Y}, relativeMapHeight:{relativeMapHeight}");
        }

        private void RebuildMesh()
        {
            boundaryPoints = GetBoundingPoints(currentMap, 1);

            List<UnityVector3> vertices = new List<UnityVector3>();
            List<int> triangles = new List<int>();


            foreach (var p in boundaryPoints)
            {
                vertices.Add(new UnityVector3(p.X, p.Y, 0));
            }
            triangles.AddRange(GetTriangleList(boundaryPoints));


            Mesh mesh = new();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);

            mesh.name = "MeshGeneratedByScript";
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();


            var meshFilter = GetComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;
            var meshCollider = GetComponent<MeshCollider>();
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = mesh;
        }

        private static List<int> BoostTriangles(List<int> originalTriangles, int numBoundaryPoints)
        {

            List<int> finalTriangles = new List<int>();
            for (int i = 0; i < originalTriangles.Count; i++)
            {
                finalTriangles.Add(originalTriangles[i]);
            }

            // Back Face Triangles (Offset by numBoundaryPoints and reverse winding order for backface culling)
            for (int i = 0; i < originalTriangles.Count; i += 3)
            {
                finalTriangles.Add(originalTriangles[i + 2] + numBoundaryPoints);
                finalTriangles.Add(originalTriangles[i + 1] + numBoundaryPoints);
                finalTriangles.Add(originalTriangles[i] + numBoundaryPoints);
            }

            // Side Edge Stitching (Connects the front face to the back face)
            for (int i = 0; i < numBoundaryPoints; i++)
            {
                int next = (i + 1) % numBoundaryPoints;

                int iFront = i;
                int nextFront = next;
                int iBack = i + numBoundaryPoints;
                int nextBack = next + numBoundaryPoints;

                // First triangle of the side quad
                finalTriangles.Add(iFront);
                finalTriangles.Add(nextFront);
                finalTriangles.Add(iBack);

                // Second triangle of the side quad
                finalTriangles.Add(nextFront);
                finalTriangles.Add(nextBack);
                finalTriangles.Add(iBack);
            }
            return finalTriangles;
        }

        private static List<int> GetTriangleList(List<Vector2> boundingPoints)
        {
            List<int> indexList = new List<int>();
            for (int i = 0; i < boundingPoints.Count; i++)
            {
                indexList.Add(i);
            }
            List<int> triangleList = new List<int>();


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
            List<Vector2> boundingPoints = new();
            var texture = mapDefinition.TextureMap;
            int width = texture.GetLength(0);
            int height = texture.GetLength(1);

            // Track the absolute floor level and the maximum column width bounds
            int startX = 0;
            int endX = width - 1;


            for (int x = startX; x <= endX; x++)
            {
                // Scan down from the top of the map to find the highest solid pixel
                int surfaceY = 0;
                for (int y = height - 1; y >= 0; y--)
                {
                    if (texture[x, y] == material)
                    {
                        surfaceY = y;
                        break;
                    }
                }
                boundingPoints.Add(new Vector2(x, surfaceY));
            }

            boundingPoints.Add(new Vector2(endX, 0));

            for (int x = endX - 1; x > startX; x--)
            {
                boundingPoints.Add(new Vector2(x, 0));
            }

            boundingPoints.Add(new Vector2(startX, 0));

            return boundingPoints;
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


        private static int[,] GenerateTestEnvironment(int material, Vector2Int size)
        {
            int width = size.X;
            int height = size.Y;

            int[,] texture = new int[width, height];

            // Random seed: each call produces a different landscape
            float seed = Random.Range(0f, 1000f);

            for (int x = 0; x < width; x++)
            {
                //smaller perlinNoise factor == less frequent
                float primaryHill = Mathf.PerlinNoise((x * 0.003f) + seed, seed) * 0.7f;
                float gentleBumps = Mathf.PerlinNoise((x * 0.015f) + seed, seed + 100f) * 0.15f;

                float noise = primaryHill + gentleBumps;


                float columnHeight = noise * height * 0.55f;
                columnHeight = Mathf.Clamp(columnHeight, 1, height * 0.9f);

                // Fill from the bottom up: ground sits at the bottom of the texture
                for (int y = 0; y < columnHeight; y++)
                {
                    texture[x, y] = material;
                }
            }
            return texture;
        }

        public static void ExportToPNG(int[,] grid, string fileName)
        {
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);

            // 1. Create a new Texture2D
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Point; // Keeps pixels sharp if you zoom in
            texture.wrapMode = TextureWrapMode.Clamp;

            // 2. Loop through the array and map integers to pixels
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int value = grid[x, y];

                    // Fallback to magenta if the integer goes beyond your provided colors array
      
                    Color pixelColor = (value > 0) ? Color.black : Color.white;

                    texture.SetPixel(x, y, pixelColor);
                }
            }

            // 3. Apply structural changes to the texture
            texture.Apply();

            // 4. Convert the texture into PNG bytes
            byte[] pngData = ImageConversion.EncodeToPNG(texture);

            // 5. Save the file to disk
            string filePath = Path.Combine(Application.dataPath, fileName);
            //File.Delete(filePath);
            File.WriteAllBytes(filePath, pngData);

            // Clean up memory
            Object.DestroyImmediate(texture);

            Debug.Log($"Image successfully exported to: {filePath}");
        }

        private static int WrapIndex(int index, int count)
        {
            return (index % count + count) % count;
        }

    }
}
