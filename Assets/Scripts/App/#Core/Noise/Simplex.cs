using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{

    [CreateAssetMenu(fileName = "Simplex", menuName = "Noise/Simplex", order = 2)]
    public class Simplex : NoiseModel, INoise
    {

        private int[] A = new int[3];
        private float s, u, v, w;
        private int i, j, k;
        private float onethird = 0.333333333f;
        private float onesixth = 0.166666667f;
        private int[] T;

        public Simplex()
        {
            if (T == null)
            {
                System.Random rand = new System.Random();
                T = new int[8];
                for (int q = 0; q < 8; q++)
                    T[q] = rand.Next();
            }
        }

        public Simplex(string seed)
        {
            T = new int[8];
            string[] seed_parts = seed.Split(new char[] { ' ' });

            for (int q = 0; q < 8; q++)
            {
                int b;
                try
                {
                    b = int.Parse(seed_parts[q]);
                }
                catch
                {
                    b = 0x0;
                }
                T[q] = b;
            }
        }

        public Simplex(int[] seed)
        { // {0x16, 0x38, 0x32, 0x2c, 0x0d, 0x13, 0x07, 0x2a}
            T = seed;
        }



        public string GetSeed()
        {
            string seed = "";

            for (int q = 0; q < 8; q++)
            {
                seed += T[q].ToString();
                if (q < 7)
                    seed += " ";
            }

            return seed;
        }

        protected override float Noise2D(float x, float y)
            => Noise3D(x, y, 0.0f);

        protected override float Noise3D(float x, float y, float z)
        {
            s = (x + y + z) * onethird;
            i = Fastfloor(x + s);
            j = Fastfloor(y + s);
            k = Fastfloor(z + s);

            s = (i + j + k) * onesixth;
            u = x - i + s;
            v = y - j + s;
            w = z - k + s;

            A[0] = 0; A[1] = 0; A[2] = 0;

            int hi = u >= w ? u >= v ? 0 : 1 : v >= w ? 1 : 2;
            int lo = u < w ? u < v ? 0 : 1 : v < w ? 1 : 2;

            return Kay(hi) + Kay(3 - hi - lo) + Kay(lo) + Kay(0);
        }


        private float Kay(int a)
        {
            s = (A[0] + A[1] + A[2]) * onesixth;
            float x = u - A[0] + s;
            float y = v - A[1] + s;
            float z = w - A[2] + s;
            float t = 0.6f - x * x - y * y - z * z;
            int h = Shuffle(i + A[0], j + A[1], k + A[2]);
            A[a]++;
            if (t < 0) return 0;
            int b5 = h >> 5 & 1;
            int b4 = h >> 4 & 1;
            int b3 = h >> 3 & 1;
            int b2 = h >> 2 & 1;
            int b1 = h & 3;

            float p = b1 == 1 ? x : b1 == 2 ? y : z;
            float q = b1 == 1 ? y : b1 == 2 ? z : x;
            float r = b1 == 1 ? z : b1 == 2 ? x : y;

            p = b5 == b3 ? -p : p;
            q = b5 == b4 ? -q : q;
            r = b5 != (b4 ^ b3) ? -r : r;
            t *= t;
            return 8 * t * t * (p + (b1 == 0 ? q + r : b2 == 0 ? q : r));
        }

        private int Shuffle(int i, int j, int k)
        {
            return Vect(i, j, k, 0) + Vect(j, k, i, 1) + Vect(k, i, j, 2) + Vect(i, j, k, 3) + Vect(j, k, i, 4) + Vect(k, i, j, 5) + Vect(i, j, k, 6) + Vect(j, k, i, 7);
        }

        private int Vect(int i, int j, int k, int B)
        {
            return T[Vect(i, B) << 2 | Vect(j, B) << 1 | Vect(k, B)];
        }

        private int Vect(int N, int B)
        {
            return N >> B & 1;
        }

        private int Fastfloor(float n)
        {
            return n > 0 ? (int)n : (int)n - 1;
        }


    }
}