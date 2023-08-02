using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshHandler
{

    public static Mesh CreateMesh(Vector2Int resolution)
        => CreateMesh(resolution.x, resolution.y);



    public static Mesh CreateMesh(int width, int length)
    {
        var numVert = (width + 1) * (length + 1);
        var numTri = (width + 1) * (length + 1) * 6;

        var vList = new List<Vector3>(numVert);
        for (int z = 0; z < length + 1; z++)
            for (int x = 0; x < width + 1; x++)
                vList.Add(new Vector3(x, 0, z));


        var uvList = new List<Vector2>(numVert);
        for (int y = 0; y < length + 1; y++)
            for (int x = 0; x < width + 1; x++)
                uvList.Add(new Vector2((float)x / width, (float)y / length));

        var tList = new List<int>(numTri);
        for (int vert = 0, z = 0; z < length; z++, vert++)
        {
            for (int x = 0; x < width; x++, vert++)
            {
                tList.Add(vert + 0);
                tList.Add(vert + width + 1);
                tList.Add(vert + 1);
                tList.Add(vert + 1);
                tList.Add(vert + width + 1);
                tList.Add(vert + width + 2);
            }
        }


        var mesh = new Mesh();
        mesh.name = "Custom mesh";
        mesh.vertices = vList.ToArray();
        mesh.triangles = tList.ToArray();
        mesh.uv = uvList.ToArray();


        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    public static Mesh CreateMesh(float[,] heightMap, AnimationCurve curve, float heightMultiplier = 1f)
    {

        var width = heightMap.GetLength(0);
        var length = heightMap.GetLength(1);


        var numVert = width * length;
        var numTri = width * length * 6;

        var vList = new List<Vector3>(numVert);
        for (int z = 0; z < length; z++)
            for (int x = 0; x < width; x++)
                vList.Add(new Vector3(x, curve.Evaluate(heightMap[x, z]) * heightMultiplier, z));



        var uvList = new List<Vector2>(numVert);
        for (int y = 0; y < length; y++)
            for (int x = 0; x < width; x++)
                uvList.Add(new Vector2((float)x / width, (float)y / length));



        var tList = new List<int>(numTri);
        for (int vert = 0, z = 0; z < length - 1; z++, vert++)
        {
            for (int x = 0; x < width - 1; x++, vert++)
            {
                tList.Add(vert + 0);
                tList.Add(vert + width);
                tList.Add(vert + 1);

                tList.Add(vert + 1);
                tList.Add(vert + width);
                tList.Add(vert + width + 1);
            }
        }



        var mesh = new Mesh();
        mesh.name = "Custom mesh";
        mesh.vertices = vList.ToArray();
        mesh.triangles = tList.ToArray();
        mesh.uv = uvList.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;

    }
}
