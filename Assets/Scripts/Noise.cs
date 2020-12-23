using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise 
{

    public enum NormalizeMode { Local, Global };
    public static float [,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode)
    {
        //prepare the array of values
        float[,] noisemap = new float[mapWidth, mapHeight];

        //set up pseudo random generator
        System.Random prng = new System.Random(seed);
        
        //prepare octave offsets
        Vector2[] octaveOffsets = new Vector2[octaves];

        //iteration variables
        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        //set up octave offsets 
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-10000, 10000) + offset.x;
            float offsetY = prng.Next(-10000, 10000) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        if (scale <= 0 ){
            scale = 0.0001f;
        }

        float minLocalNoiseHeight = float.MaxValue;
        float maxLocalNoiseHeight = float.MinValue;

        float halfWidth = mapWidth / 2f;
        float halfheight = mapHeight / 2f;

        //columns
        for (int y=0; y < mapHeight; y++)
        {
            //rows
            for (int x = 0; x < mapWidth; x++)
            {
                //individual values
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                //iterate through octaves
                for (int i = 0; i< octaves; i++) {
                    float sampleX = (x-halfWidth + octaveOffsets[i].x) / scale * frequency ;
                    float sampleY = (y-halfheight + octaveOffsets[i].y) / scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY)* 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                //if this noise height is the largest, store it
                if (noiseHeight > maxLocalNoiseHeight)
                {
                    maxLocalNoiseHeight = noiseHeight;
                }
                
                if (noiseHeight < minLocalNoiseHeight)
                //if this noise height is the smallest, store that
                {
                    minLocalNoiseHeight = noiseHeight;
                }

                //set the noise value at current cursor
                noisemap[x, y] = noiseHeight;
            }
        }


        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (normalizeMode == NormalizeMode.Local) { 
                    noisemap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noisemap[x, y]);
                }
                else
                {
                    float normalizedHeight = (noisemap[x, y] + 1) / (2f * maxPossibleHeight / 1.35f);
                    noisemap[x, y] = Mathf.Clamp(normalizedHeight, 0 , int.MaxValue);
                }
            }
        }

        return noisemap;
    }
}
