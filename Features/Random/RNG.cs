using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public class RNG
	{
		int[] perlinPermutations = { 151,160,137,91,90,15,
		131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
		190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
		88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
		77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
		102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
		135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
		5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
		223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
		129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
		251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
		49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
		138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
	};
		int[] p;

		int _seed;
		int _next;

		public RNG(int seed)
		{
			_seed = seed;

			p = new int[512];
			for (int i = 0; i < 256; i++)
				p[256 + i] = p[i] = perlinPermutations[i];
		}

		public float Next()
		{
			var random = Value(_next);
			_next++;
			return random;
		}

		public float Range(float min, float max)
		{
			var random = Next();
			return min + (max - min) * random;
		}

		public int Range(int min, int max)
		{
			var random = Next();
			return (int)(min + (max - min) * random);
		}

		public float Range(Vector2 value)
		{
			var random = Next();
			return value.x + (value.y - value.x) * random;
		}

		public float Value(int value)
		{
			long h = _seed * 3741761393 + value * 6682615263;
			h = (h ^ (h << 13)) * 127412261177;
			return (h ^ (h >> 16)) / (float)long.MaxValue;
		}

		public float Value(float value) => Value(Int(value));

		public float Value(int x, int y)
		{
			long h = _seed * 110563 + x * 374761393 + y * 668265263;
			h = (h ^ (h << 24)) * 1274126177;
			h = (h ^ (h >> 9)) * 47976233;
			return (h ^ (h >> 16)) / (float)long.MaxValue;
		}

		public float Value(float x, float y) => Value(Int(x), Int(y));

		public float Value(Vector2 v2) => Value(v2.x, v2.y);

		public float Value(int x, int y, int z)
		{
			long h = _seed * 110563 + x * 374761393 + y * 668265263 + z * 32453203;
			h = (h ^ (h >> 13)) * 1274126177;
			h = (h ^ (h << 32)) * 47976233;
			h = (h ^ (h >> 12)) * 49979687;
			return (h ^ (h >> 16)) / (float)long.MaxValue;
		}

		public float Value(float x, float y, float z) => Value(Int(x), Int(y), Int(z));

		public float Value(Vector3 v3) => Value(v3.x, v3.y, v3.z);

		public int Range(int min, int max, int i)
		{
			var range = max - min;
			var v = Value(i);
			return (int)(v * range) + min;
		}

		unsafe int Int(float f)
		{
			return *(int*)&f;
		}

		public Vector2 RandomVector2(int x, int y)
		{
			return new Vector2(Value(x) + Value(x + y) - 1, Value(y) + Value(y * x) - 1);
		}

		public Vector2 InsideCircle()
		{
			return new Vector2(Next() * 2 - 1, Next() * 2 - 1).normalized * 0.5f;
		}

		//Perlin

		public float RidgedNoise(float x, float y, int octaves, float frequency, out float noABS)
		{
			float total = 0;

			for (int i = 0; i < octaves; i++)
			{
				float perl = PerlinNoise(x * frequency + i * 60, y * frequency - i * 60);

				total += perl;
			}

			noABS = total / (float)octaves;

			return Mathf.Abs(noABS);
		}

		public float PerlinNoiseOctaves(float x, float y, int octaves, float persistance, float frequency, bool noise = false, float noiseScale = 0.1f)
		{
			float total = 0;
			float maxTotal = 0;
			float amplitude = 1;
			for (int i = 0; i < octaves; i++)
			{
				total += PerlinNoise01((x + amplitude * i) * frequency, (y + amplitude * i) * frequency) * amplitude;
				maxTotal += amplitude;
				amplitude *= persistance;
				frequency *= 2;
			}

			if (noise)
			{
				amplitude *= 0.5f;

				total += PerlinNoise01((x + amplitude * octaves) * noiseScale, (y + amplitude * octaves) * noiseScale) * amplitude;
				maxTotal += amplitude;
			}

			var p = total / maxTotal;

			//return p;

			//parametric ease in-out 
			//return p * p / (2f * (p * p - p) + 1f);

			//bezier ease in-out
			//return p * p * (3 - 2 * p);

			//cubic
			//return p < 0.5f ? 4f * p * p * p : (p - 1) * (2 * p - 2) * (2 * p - 2) + 1f;

			//quint
			return p < 0.5f ? 16 * p * p * p * p * p : 1 + 16 * (--p) * p * p * p * p;
		}

		public float PerlinNoise01(float x, float y)
		{
			float p = PerlinNoise(x, y);
			return (p + 1) * 0.5f;
		}

		public float PerlinNoise(float x, float y)
		{
			float z = _seed * 0.0001f;

			x = Mathf.Abs(x);
			y = Mathf.Abs(y);
			z = Mathf.Abs(z);

			int X = (int)x & 255;
			int Y = (int)y & 255;
			int Z = (int)z & 255;
			x -= (int)x;
			y -= (int)y;
			z -= (int)z;
			float u = fade(x);
			float v = fade(y);
			float w = fade(z);
			int A = p[X] + Y;
			int AA = p[A] + Z;
			int AB = p[A + 1] + Z;
			int B = p[X + 1] + Y;
			int BA = p[B] + Z;
			int BB = p[B + 1] + Z;

			float f1 = lerp(u, grad(p[AA], x, y, z), grad(p[BA], x - 1, y, z));
			float f2 = lerp(u, grad(p[AB], x, y - 1, z), grad(p[BB], x - 1, y - 1, z));
			float g1 = lerp(v, f1, f2);

			float f3 = lerp(u, grad(p[AA + 1], x, y, z - 1), grad(p[BA + 1], x - 1, y, z - 1));
			float f4 = lerp(u, grad(p[AB + 1], x, y - 1, z - 1), grad(p[BB + 1], x - 1, y - 1, z - 1));
			float g2 = lerp(v, f3, f4);

			return lerp(w, g1, g2);
		}

		float fade(float f)
			=> f * f * f * (f * (f * 6 - 15) + 10);

		float lerp(float t, float x, float y)
			=> x + t * (y - x);

		float grad(int hash, float x, float y, float z)
		{
			int h = hash & 15;
			float u = h < 8 ? x : y;
			float v = h < 4 ? y : h == 12 || h == 14 ? x : z;
			return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
		}
	}
}
