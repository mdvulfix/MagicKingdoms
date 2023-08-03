using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public static class TextureHandler
    {

        public static Texture2D GetTextureFromNoise(int width, int length, INoise noise, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Clamp)
        {

            var texture = new Texture2D(width, length);

            var pixels = new Color[width * length];

            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < width; x++)
                {


                    var colorNoiseValue = noise != null ? noise.Noise2D(x, y) : 255;
                    pixels[y * width + x] = new Color(colorNoiseValue, colorNoiseValue, colorNoiseValue);
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
            texture.filterMode = filterMode;
            texture.wrapMode = wrapMode;


            return texture;
        }

        public static Texture2D GetTextureShapeMask(int width, int length, float radius, TextureShape textureShape = TextureShape.None, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Clamp)
        {

            var texture = new Texture2D(width, length);

            var center = new Vector2(width / 2f, length / 2f);
            //var radius = Mathf.Min(width, length) * 0.5f;

            var pixels = new Color[width * length];
            var noise = new Simplex();


            switch (textureShape)
            {
                case TextureShape.None:

                    var scale = Mathf.Min(width, length);
                    var heightMap = noise.GetMatrix2D(new Vector2Int(width, length), Vector2.zero, scale);

                    for (int y = 0; y < length; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {

                            var distance = 0.0f;
                            var point = new Vector2(x, y);

                            if ((distance = Vector2.Distance(point, center)) <= radius)
                                pixels[y * width + x] = new Color(255, 255, 255);
                            else
                                pixels[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y] * 1.5f);


                        }
                    }

                    break;

                case TextureShape.Circle:

                    for (int y = 0; y < length; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            var distance = 0.0f;
                            var point = new Vector2(x, y);
                            if ((distance = Vector2.Distance(point, center)) <= radius)
                                pixels[y * width + x] = new Color(255, 255, 255);
                            else
                                pixels[y * width + x] = new Color(0, 0, 0);
                        }
                    }

                    break;
            }






            texture.SetPixels(pixels);
            texture.Apply();
            texture.filterMode = filterMode;
            texture.wrapMode = wrapMode;


            return texture;
        }

    }

    public enum TextureShape
    {
        None,
        Circle,
        Square
    }
}