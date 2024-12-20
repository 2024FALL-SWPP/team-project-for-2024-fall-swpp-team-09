using System;

// 일부 메서드는 Python의 구현을 참고해서 구현함
// https://github.com/python/cpython/blob/3.13/Lib/random.py
public class SCH_Random : Random
{
    /*************
     * constants *
     *************/

    // 4.0 * Math.Exp(-0.5) / Math.Sqrt(2.0)
    private const double NORMAL_MAGICCONST = 1.7155277699214135367355993366800248622894287109375;

    /***********
     * methods *
     ***********/

    // 조합
    public int[] Combination(int n, int r)
    {
        int[] candidates = new int[n];
        int[] result = new int[r];

        for (int i = 0; i < n; i++) {
            candidates[i] = i;
        }

        for (int i = 0; i < r; i++) {
            int index = Next(i, n);

            result[i] = candidates[index];
            if (index != i) {
                int tmp = candidates[i];

                candidates[i] = candidates[index];
                candidates[index] = tmp;
            }
        }

        System.Array.Sort(result);

        return result;
    }

    // 로그 정규 분포
    public double LogNormalDist(double mu, double sigma)
    {
        return Math.Exp(NormalDist(mu, sigma));
    }

    // 정규 분포
    public double NormalDist(double mu = 0.0, double sigma = 1.0)
    {
        double u1, u2, z, zz;

        while (true) {
            u1 = Sample();
            u2 = 1.0 - Sample();
            z = NORMAL_MAGICCONST * (u1 - 0.5) / u2;
            zz = z * z / 4.0;
            if (zz <= -Math.Log(u2)) {
                break;
            }
        }

        return mu + z * sigma;
    }

    // 순열
    public int[] Permutation(int n, int r)
    {
        int[] candidates = new int[n];
        int[] result = new int[r];

        for (int i = 0; i < n; i++) {
            candidates[i] = i;
        }

        for (int i = 0; i < r; i++) {
            int index = Next(i, n);

            result[i] = candidates[index];
            if (index != i) {
                int tmp = candidates[i];

                candidates[i] = candidates[index];
                candidates[index] = tmp;
            }
        }

        return result;
    }

    // 삼각 분포
    public double TriangularDist(double low = 0.0, double high = 1.0, double mode = 0.5)
    {
        double u, c;

        if (high == low) {
            return low;
        }

        u = Sample();
        c = (mode - low) / (high - low);

        if (u > c) {
            double tmp = low;

            u = 1.0 - u;
            c = 1.0 - c;
            low = high;
            high = tmp;
        }

        return low + (high - low) * Math.Sqrt(u * c);
    }

    // 균등 분포
    public double UniformDist(double a, double b)
    {
        return a + (b - a) * Sample();
    }
}
