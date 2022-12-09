using System;
using System.Numerics;

namespace NURBS
{
    /// <summary>
    /// Control point used by NURBS surface with coordinates and weight
    /// </summary>
    [Serializable]
    public class ControlPoint
    {
        /// <summary>
        /// Coordinates of the control point
        /// </summary>
        public float x, y, z;
        /// <summary>
        /// Weight of the control point, how much influence it has
        /// </summary>
        public float weight = 1;

        public ControlPoint(float x, float y, float z, float weight)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.weight = weight;
        }
    }

    public class Surface
    {
        /// <summary>
        /// Control points used by the NURBS surface.<br/>
        /// Can be changed at any time and will reflected on the next BuildMesh()
        /// </summary>
        public ControlPoint[,] controlPoints;

        private int degreeU, degreeV;

        /// <summary>
        /// Change degree in U axis and new knot vector.
        /// </summary>
        /// <param name="degree">Degree U</param>
        /// <param name="knots">Knot vector in U</param>
        public void DegreeU(int degree, float[] knots)
        {
            this.degreeU = degree;
            this.knotsU = knots;
        }
        /// <summary>
        /// Change degree in U axis. Knot vector is automatically generated
        /// </summary>
        /// <param name="degree">Degree U</param>
        public void DegreeU(int degree)
        {
            this.degreeU = degree;
            this.knotsU = GenerateKnots(degree, controlPoints.GetLength(0), true, true);
        }

        /// <summary>
        /// Change degree in V axis and new knot vector.
        /// </summary>
        /// <param name="degree">Degree V</param>
        /// <param name="knots">Knot vector in V</param>
        public void DegreeV(int degree, float[] knots)
        {
            this.degreeV = degree;
            this.knotsV = knots;
        }
        /// <summary>
        /// Change degree in V axis. Knot vector is automatically generated
        /// </summary>
        /// <param name="degree">Degree U</param>
        public void DegreeV(int degree)
        {
            this.degreeV = degree;
            this.knotsV = GenerateKnots(degree, controlPoints.GetLength(1), true, true);
        }

        /// <summary>
        /// Knot vectors of the NURBS surface.<br/>
        /// A sequence of parameter values that determines where and how the control points affect the NURBS curve.<br/>
        /// Length must be number of control points + degree + 1 in that direction.<br/>
        /// Can be changed at any time and will reflected on the next BuildMesh()
        /// </summary>
        public float[] knotsU, knotsV;

        /// <summary>
        /// Build a NURBS surface with desired control points and UV degrees.<br/>
        /// Each degree must be &lt; number of control points in respective direction. ie: degreeU &lt; controlPoints.GetLength(0).<br/>
        /// Knot vectors are not specified and will be generated automatically with start and end clamps, check GenerateKnots for more advanced uses
        /// </summary>
        /// <param name="controlPoints">2D grid of control points with coordinates and weight</param>
        /// <param name="degreeU">Degree in U, number of nearby control points that influence any given point on the curve</param>
        /// <param name="degreeV">Degree in V, number of nearby control points that influence any given point on the curve</param>
        public Surface(ControlPoint[,] controlPoints, int degreeU, int degreeV)
        {

            this.degreeU = degreeU;
            this.degreeV = degreeV;

            this.controlPoints = controlPoints;

            //No knots provided, generate
            this.knotsU = GenerateKnots(degreeU, controlPoints.GetLength(0), true, true);
            this.knotsV = GenerateKnots(degreeV, controlPoints.GetLength(1), true, true);
        }

        /// <summary>
        /// Build a NURBS surface with desired control points and UV degrees.<br/>
        /// [Each degree must be &lt; number of control points in respective direction. ie: degreeU &lt; controlPoints.GetLength(0).<br/>
        /// Each knot vector length must be == number of control points in respective direction + degree + 1. ie: knotsU.Length == controlPoints.GetLength(0) + degreeU + 1
        /// </summary>
        /// <param name="controlPoints">2D grid of control points with coordinates and weight</param>
        /// <param name="degreeU">Degree in U, number of nearby control points that influence any given point on the curve</param>
        /// <param name="degreeV">Degree in V, number of nearby control points that influence any given point on the curve</param>
        /// <param name="knotsU">Knot vector in U, sequence of parameter values that determines where and how the control points affect the NURBS curve</param>
        /// <param name="knotsV">Knot vector in V, sequence of parameter values that determines where and how the control points affect the NURBS curve</param>
        public Surface(ControlPoint[,] controlPoints, int degreeU, int degreeV, float[] knotsU, float[] knotsV)
        {

            this.degreeU = degreeU;
            this.degreeV = degreeV;

            this.controlPoints = controlPoints;

            this.knotsU = knotsU;
            this.knotsV = knotsV;
        }

        private Vector3[] vertices;
        private int[] tris;
        private Vector2[] uvs;
        public Vector2 UVPoint1;
        public Vector2 UVPoint2;
        public Vector2 UVPoint3;
        public Vector2 UVPoint4;

        /// <summary>
        /// Generates and returns a Mesh from the NURBS surface with a specified resolution
        /// </summary>
        /// <param name="resolutionU">Number of segments sampled in the U axis</param>
        /// <param name="resolutionV">Number of segments sampled in the V axis</param>
        /// <param name="mesh">Mesh to modify. If not provided (null) a new one is generated</param>
        /// <param name="calculateTangents">Calculate tangents for the mesh. Tangents are only needed for shaders that use normal maps</param>
        /*public Mesh BuildMesh(int resolutionU, int resolutionV, Mesh mesh = null, bool calculateTangents = false)
        {
            //No mesh provided
            if (mesh == null)
                mesh = new Mesh();

            if (vertices == null || vertices.Length != (resolutionU + 1) * (resolutionV + 1))
                vertices = new Vector3[(resolutionU + 1) * (resolutionV + 1)];
            if (uvs == null || uvs.Length != (resolutionU + 1) * (resolutionV + 1))
                uvs = new Vector2[(resolutionU + 1) * (resolutionV + 1)];
            int ct = 0;

            for (int a = 0; a <= resolutionU; a++)
            {
                float e = (float)a / resolutionU;
                for (int b = 0; b <= resolutionV; b++)
                {
                    float d = (float)b / resolutionV;
                    Vector3 c = GetPoint(d, e);
                    vertices[ct] = c;
                    uvs[ct].x = e;
                    uvs[ct].y = d;
                    ct++;
                }
            }

            if (tris == null || tris.Length != resolutionU * resolutionV * 6)
                tris = new int[resolutionU * resolutionV * 6];
            ct = 0;
            int f = resolutionV + 1;
            for (int a = 0; a < resolutionU; a++)
                for (int b = 0; b < resolutionV; b++)
                {
                    int h = a * f + b;
                    int i = a * f + b + 1;
                    int j = (a + 1) * f + b + 1;
                    int k = (a + 1) * f + b;
                    tris[ct] = (k);
                    tris[ct + 2] = (i);
                    tris[ct + 1] = (h);
                    tris[ct + 3] = (k);
                    tris[ct + 5] = (j);
                    tris[ct + 4] = (i);
                    ct += 6;
                }

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = tris;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
            if (calculateTangents)
                mesh.RecalculateTangents();
            mesh.RecalculateBounds();

            return mesh;
        }*/
        public Vector3[] ReturnVertices(int resolutionU, int resolutionV, bool calculateTangents = false)
        {
            //No mesh provided

            if (vertices == null || vertices.Length != (resolutionU + 1) * (resolutionV + 1))
                vertices = new Vector3[(resolutionU + 1) * (resolutionV + 1)];
            if (uvs == null || uvs.Length != (resolutionU + 1) * (resolutionV + 1))
                uvs = new Vector2[(resolutionU + 1) * (resolutionV + 1)];
            int ct = 0;

            for (int a = 0; a <= resolutionU; a++)
            {
                float e = (float)a / resolutionU;
                for (int b = 0; b <= resolutionV; b++)
                {
                    float d = (float)b / resolutionV;
                    Vector3 c = GetPoint(d, e);
                    vertices[ct] = c;
                    ct++;
                }
            }

            return vertices;
        }

        /// <summary>
        /// Generate a balanced knot vector for specified amount of control points and degree with optional clamps.<br/>
        /// The resulting knot vector will have length nControlPoints + degree + 1.<br/>
        /// Clamps ensure that the curve starts on the first control point and/or end on the last control point.<br/>
        /// Clamps set the first and/or last degree + 1 positions of the knot vector to the same value. 0 for start clamp and 1 for end clamp
        /// </summary>
        /// <param name="degree">Degree of the curve</param>
        /// <param name="controlPoints">Number of control points</param>
        /// <param name="clampStart">Clamp curve to first control point</param>
        /// <param name="clampEnd">Clamp curve to last control point</param>
        public static float[] GenerateKnots(int degree, int controlPoints, bool clampStart, bool clampEnd)
        {
            int n = degree + 1;

            //Default knots
            float[] knots = new float[n + controlPoints];
            for (int i = 0; i < knots.Length; i++)
            {
                knots[i] = -1;
            }

            //Apply clamping
            int start = clampStart ? degree + 1 : 0 + 1;
            int end = clampEnd ? knots.Length - 1 - (degree + 1) : knots.Length - 1 - 1;
            for (int i = 0; i < knots.Length; i++)
            {
                if (i < start)
                    knots[i] = 0;
                else if (i > end)
                    knots[i] = 1;
            }

            //Count unclamped
            int ct = 0;
            for (int i = 0; i < knots.Length; i++)
            {
                if (knots[i] == -1)
                    ct++;
            }

            //Calculated unclamped knots value
            float step = 1f / (ct + 1);
            int j = 1;
            for (int i = 0; i < knots.Length; i++)
            {
                if (knots[i] == -1)
                {
                    knots[i] = j * step;
                    j++;
                }
            }

            return knots;
        }

        private int Span(int a, float b, float[] c)
        {
            int d = c.Length - a - 1;
            if (b >= c[d]) return d - 1;
            if (b <= c[a]) return a;
            int e = a;
            int f = d;
            int g = (int)((e + f) / 2f); //Floor to int using cast (non negative numbers)
            while (b < c[g] || b >= c[g + 1])
            {
                if (b < c[g]) f = g;
                else e = g;
                g = (int)((e + f) / 2f);
            }
            return g;
        }

        /// <summary>
        /// Get relative object coordinates of surface point from UV coordinates.<br/>
        /// Note these are the coordinates relative to the surface control points origin
        /// </summary>
        /// <param name="u">U coordinate along the surface [0, 1]</param>
        /// <param name="v">V coordinate along the surface [0, 1]</param>
        public Vector3 GetPoint(float u, float v)
        {

            float firstU = knotsU[0];
            float firstV = knotsV[0];
            float c = firstU + u * (knotsU[knotsU.Length - 1] - firstU);
            float d = firstV + v * (knotsV[knotsV.Length - 1] - firstV);
            float[] vec = SurfacePoint(c, d);
            return new Vector3(vec[0], vec[1], vec[2]);
        }

        float[][] l;
        float[] p = new float[4];
        private float[] SurfacePoint(float f, float g)
        {
            int h = Span(degreeU, f, knotsU);
            int i = Span(degreeV, g, knotsV);
            float[] j = Basis1(h, f, degreeU, knotsU);
            float[] k = Basis2(i, g, degreeV, knotsV);

            if (l == null || l.Length < degreeV + 1)
            {
                l = new float[degreeV + 1][];
                for (int a = 0; a < degreeV + 1; a++)
                {
                    l[a] = new float[4];
                }
            }

            int id = i - degreeV;
            int hd = h - degreeU;
            for (int m = 0; m <= degreeV; ++m)
            {
                float[] curr = l[m];
                curr[0] = 0;
                curr[1] = 0;
                curr[2] = 0;
                curr[3] = 0;

                int t = id + m;
                for (int n = 0; n <= degreeU; ++n)
                {
                    ControlPoint cp = controlPoints[hd + n, t];
                    float w = cp.weight;

                    p[0] = cp.x * w;
                    p[1] = cp.y * w;
                    p[2] = cp.z * w;
                    p[3] = w;

                    p[0] *= j[n];
                    p[1] *= j[n];
                    p[2] *= j[n];
                    p[3] *= j[n];

                    curr[0] += p[0];
                    curr[1] += p[1];
                    curr[2] += p[2];
                    curr[3] += p[3];
                }
            }

            p[0] = 0;
            p[1] = 0;
            p[2] = 0;
            p[3] = 0;
            for (int m = 0; m <= degreeV; ++m)
            {
                l[m][0] *= k[m];
                l[m][1] *= k[m];
                l[m][2] *= k[m];
                l[m][3] *= k[m];

                p[0] += l[m][0];
                p[1] += l[m][1];
                p[2] += l[m][2];
                p[3] += l[m][3];
            }

            p[0] /= p[3];
            p[1] /= p[3];
            p[2] /= p[3];

            return p;
        }

        float[] e1;
        float[] f1;
        float[] g1;
        private float[] Basis1(int a, float b, int degree, float[] knots)
        {
            if (e1 == null || e1.Length != degree + 1)
                e1 = new float[degree + 1];
            if (f1 == null || f1.Length != degree + 1)
                f1 = new float[degree + 1];
            if (g1 == null || g1.Length != degree + 1)
                g1 = new float[degree + 1];

            e1[0] = 1f;
            for (int h = 1; h <= degree; ++h)
            {
                f1[h] = b - knots[a + 1 - h];
                g1[h] = knots[a + h] - b;
                float i = 0.0f;
                for (int j = 0; j < h; ++j)
                {
                    float k = g1[j + 1];
                    float l = f1[h - j];
                    float m = e1[j] / (k + l);
                    e1[j] = i + k * m;
                    i = l * m;
                }
                e1[h] = i;
            }
            return e1;
        }

        float[] e2;
        float[] f2;
        float[] g2;
        private float[] Basis2(int a, float b, int degree, float[] knots)
        {
            if (e2 == null || e2.Length != degree + 1)
                e2 = new float[degree + 1];
            if (f2 == null || f2.Length != degree + 1)
                f2 = new float[degree + 1];
            if (g2 == null || g2.Length != degree + 1)
                g2 = new float[degree + 1];

            e2[0] = 1f;
            for (int h = 1; h <= degree; ++h)
            {
                f2[h] = b - knots[a + 1 - h];
                g2[h] = knots[a + h] - b;
                float i = 0.0f;
                for (int j = 0; j < h; ++j)
                {
                    float k = g2[j + 1];
                    float l = f2[h - j];
                    float m = e2[j] / (k + l);
                    e2[j] = i + k * m;
                    i = l * m;
                }
                e2[h] = i;
            }
            return e2;
        }

    }

}

