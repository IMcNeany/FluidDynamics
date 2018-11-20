using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Solver : MonoBehaviour {


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void dens_step(int N, float* density, float* prevDens, float* initialVelocity, float* velocity, float diff)
    {
        AddSource(N, density, prevDens);
        Swap(prevDens, density);
        Diffuse(N, 0, density, prevDens, diff);
        Swap(prevDens, density);
        Advect(N, 0, density, prevDens, initialVelocity, velocity);
    }

    void AddSource(int N, float* density, float* source)
    {
        int size = (N + 2) * (N + 2);
        for (int i = 0; i < size; i++)
        {
            density[i] += Time.deltaTime * source[i];
        }
    }



    void Diffuse(int N, int b, float* density, float* prevDens, float diffusion)
    {
        float a = Time.deltaTime * diffusion * N * N;
        LinearSolver(N, b, density, prevDens, a, 1 + 4 * a);
    }

    void LinearSolver(int N, int b, float* density, float* prevDens, float a, float c)
    {
        for (int k = 0; k < 20; k++)
        {

            for (int i = 0; i <= N; i++)
            {
                for (int j = 1; j <= N; j++)
                {
                    density[(i + (N + 2) * j)] = (prevDens[(i + (N + 2) * j)] + a * (density[((i-1) + (N + 2) * j)] +
                    density[((i+1) + (N + 2) * j)] + density[(i + (N + 2) * (j-1))] + density[(i + (N + 2) * (j+1))])) / c;
                }
            }
    
        SetBoundary(N, b, density);
        }
    }

    void SetBoundary(int N, int b, float* density)
    {
        int i;

        for (i = 1; i <= N; i++)
        {
            density[(0 +(N+2*(i)))] = b == 1 ? -density[ ((1) + (N+2)* (i))]: density[((1) + (N + 2) * (i))];

            density[((N + 1) + (N + 2) * (i))] = b == 1 ? -density[((N) + (N + 2) * (i))]: density[((N) + (N + 2) * (i))];

            density[(i + (N+2)* (0))] = b == 2 ? -density[(i +(N+2)*1)] : density[(i + (N + 2) * 1)];

            density[(i + (N+2) * (N+1))] = b == 2 ? -density[(i + (N+2) * (N))] : density[(i + (N + 2) * (N))];
        }
        density[(0 + (N + 2) * (0))] = 0.5f * (density[(1 + (N + 2) * (0))] + density[(0 + (N + 2) * (1))]);
        density[(0 + (N + 2) * (N+1))] = 0.5f * (density[(1 + (N + 2) * (N+1))] + density[(0 + (N + 2) * (N))]);
        density[((N+1) + (N + 2) * (0))] = 0.5f * (density[((N) + (N + 2) * (0))] + density[((N+1) + (N + 2) * (1))]);
        density[((N+1) + (N + 2) * (N+1))] = 0.5f * (density[(N + (N + 2) * (N+1))] + density[((N+1) + (N + 2) * (N))]);
    }

    void Advect(int N, int b, float* d, float* d0, float* u, float* v)
    {
        int i0, j0, i1, j1;
        float s0, t0, s1, t1, dt0;

        dt0 = Time.deltaTime * N;
        //FOR_EACH_CELL
        for (int i = 1; i <= N; i++)
        {
            for(int j =1;j<=N;j++)
            {
                float x = i - dt0 * u[(i + (N + 2) * (j))];
               float y = j - dt0 * v[(i + (N + 2) * (j))];
                if (x < 0.5f)
                {
                    x = 0.5f;
                }
                if (x > N + 0.5f)
                {
                    x = N + 0.5f;
                }
                i0 = (int)x;
                i1 = i0 + 1;

                if (y < 0.5f)
                {
                    y = 0.5f;
                }
                if (y > N + 0.5f)
                {
                    y = N + 0.5f;
                }
                j0 = (int)y;
                j1 = j0 + 1;
                s1 = x - i0;
                s0 = 1 - s1;
                t1 = y - j0;
                t0 = 1 - t1;
                d[(i + (N + 2) * (j))] = s0 * (t0 * d0[(i0 + (N + 2) * (j0))] + t1 * d0[(i0 + (N + 2) * (j1))]) +
                             s1 * (t0 * d0[(i1 + (N + 2) * (j0))] + t1 * d0[(i1 + (N + 2) * (j1))]);
            }

        }

    set_bnd(N, b, d);
    }

    void Swap (float* i, float * j)
    {
        float * tmp = i;
        i = j;
        j = tmp;
    }
}
