﻿// <copyright file="CudaLinearAlgebraProvider.Single.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
//
// Copyright (c) 2009-2015 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

#if NATIVE

using System;
using System.Security;
using Jund.MathHelper.Numerics.Properties;
using Jund.MathHelper.Numerics.Providers.Common.Cuda;

namespace Jund.MathHelper.Numerics.Providers.LinearAlgebra.Cuda
{
    /// <summary>
    /// NVidia's CUDA Toolkit linear algebra provider.
    /// </summary>
    internal partial class CudaLinearAlgebraProvider
    {
        /// <summary>
        /// Computes the dot product of x and y.
        /// </summary>
        /// <param name="x">The vector x.</param>
        /// <param name="y">The vector y.</param>
        /// <returns>The dot product of x and y.</returns>
        /// <remarks>This is equivalent to the DOT BLAS routine.</remarks>
        [SecuritySafeCritical]
        public override float DotProduct(float[] x, float[] y)
        {
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (x.Length != y.Length)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength);
            }

            return SafeNativeMethods.s_dot_product(_blasHandle, x.Length, x, y);
        }

        /// <summary>
        /// Adds a scaled vector to another: <c>result = y + alpha*x</c>.
        /// </summary>
        /// <param name="y">The vector to update.</param>
        /// <param name="alpha">The value to scale <paramref name="x"/> by.</param>
        /// <param name="x">The vector to add to <paramref name="y"/>.</param>
        /// <param name="result">The result of the addition.</param>
        /// <remarks>This is similar to the AXPY BLAS routine.</remarks>
        [SecuritySafeCritical]
        public override void AddVectorToScaledVector(float[] y, float alpha, float[] x, float[] result)
        {
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y.Length != x.Length)
            {
                throw new ArgumentException(Resources.ArgumentVectorsSameLength);
            }

            if (!ReferenceEquals(y, result))
            {
                Array.Copy(y, 0, result, 0, y.Length);
            }

            if (alpha == 0.0f)
            {
                return;
            }

            SafeNativeMethods.s_axpy(_blasHandle, y.Length, alpha, x, result);
        }

        /// <summary>
        /// Scales an array. Can be used to scale a vector and a matrix.
        /// </summary>
        /// <param name="alpha">The scalar.</param>
        /// <param name="x">The values to scale.</param>
        /// <param name="result">This result of the scaling.</param>
        /// <remarks>This is similar to the SCAL BLAS routine.</remarks>
        [SecuritySafeCritical]
        public override void ScaleArray(float alpha, float[] x, float[] result)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (!ReferenceEquals(x, result))
            {
                Array.Copy(x, 0, result, 0, x.Length);
            }

            if (alpha == 1.0f)
            {
                return;
            }

            SafeNativeMethods.s_scale(_blasHandle, x.Length, alpha, result);
        }

        /// <summary>
        /// Multiples two matrices. <c>result = x * y</c>
        /// </summary>
        /// <param name="x">The x matrix.</param>
        /// <param name="rowsX">The number of rows in the x matrix.</param>
        /// <param name="columnsX">The number of columns in the x matrix.</param>
        /// <param name="y">The y matrix.</param>
        /// <param name="rowsY">The number of rows in the y matrix.</param>
        /// <param name="columnsY">The number of columns in the y matrix.</param>
        /// <param name="result">Where to store the result of the multiplication.</param>
        /// <remarks>This is a simplified version of the BLAS GEMM routine with alpha
        /// set to 1.0f and beta set to 0.0f, and x and y are not transposed.</remarks>
        public override void MatrixMultiply(float[] x, int rowsX, int columnsX, float[] y, int rowsY, int columnsY, float[] result)
        {
            MatrixMultiplyWithUpdate(Transpose.DontTranspose, Transpose.DontTranspose, 1.0f, x, rowsX, columnsX, y, rowsY, columnsY, 0.0f, result);
        }

        /// <summary>
        /// Multiplies two matrices and updates another with the result. <c>c = alpha*op(a)*op(b) + beta*c</c>
        /// </summary>
        /// <param name="transposeA">How to transpose the <paramref name="a"/> matrix.</param>
        /// <param name="transposeB">How to transpose the <paramref name="b"/> matrix.</param>
        /// <param name="alpha">The value to scale <paramref name="a"/> matrix.</param>
        /// <param name="a">The a matrix.</param>
        /// <param name="rowsA">The number of rows in the <paramref name="a"/> matrix.</param>
        /// <param name="columnsA">The number of columns in the <paramref name="a"/> matrix.</param>
        /// <param name="b">The b matrix</param>
        /// <param name="rowsB">The number of rows in the <paramref name="b"/> matrix.</param>
        /// <param name="columnsB">The number of columns in the <paramref name="b"/> matrix.</param>
        /// <param name="beta">The value to scale the <paramref name="c"/> matrix.</param>
        /// <param name="c">The c matrix.</param>
        [SecuritySafeCritical]
        public override void MatrixMultiplyWithUpdate(Transpose transposeA, Transpose transposeB, float alpha, float[] a, int rowsA, int columnsA, float[] b, int rowsB, int columnsB, float beta, float[] c)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            if (c == null)
            {
                throw new ArgumentNullException(nameof(c));
            }

            var m = transposeA == Transpose.DontTranspose ? rowsA : columnsA;
            var n = transposeB == Transpose.DontTranspose ? columnsB : rowsB;
            var k = transposeA == Transpose.DontTranspose ? columnsA : rowsA;
            var l = transposeB == Transpose.DontTranspose ? rowsB : columnsB;

            if (c.Length != m*n)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions);
            }

            if (k != l)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions);
            }

            SafeNativeMethods.s_matrix_multiply(_blasHandle, transposeA.ToCUDA(), transposeB.ToCUDA(), m, n, k, alpha, a, b, beta, c);
        }

        /// <summary>
        /// Computes the LUP factorization of A. P*A = L*U.
        /// </summary>
        /// <param name="data">An <paramref name="order"/> by <paramref name="order"/> matrix. The matrix is overwritten with the
        /// the LU factorization on exit. The lower triangular factor L is stored in under the diagonal of <paramref name="data"/> (the diagonal is always 1.0f
        /// for the L factor). The upper triangular factor U is stored on and above the diagonal of <paramref name="data"/>.</param>
        /// <param name="order">The order of the square matrix <paramref name="data"/>.</param>
        /// <param name="ipiv">On exit, it contains the pivot indices. The size of the array must be <paramref name="order"/>.</param>
        /// <remarks>This is equivalent to the GETRF LAPACK routine.</remarks>
        [SecuritySafeCritical]
        public override void LUFactor(float[] data, int order, int[] ipiv)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (ipiv == null)
            {
                throw new ArgumentNullException(nameof(ipiv));
            }

            if (data.Length != order*order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(data));
            }

            if (ipiv.Length != order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(ipiv));
            }

            Solver(SafeNativeMethods.s_lu_factor(_solverHandle, order, data, ipiv));
        }

        /// <summary>
        /// Computes the inverse of matrix using LU factorization.
        /// </summary>
        /// <param name="a">The N by N matrix to invert. Contains the inverse On exit.</param>
        /// <param name="order">The order of the square matrix <paramref name="a"/>.</param>
        /// <remarks>This is equivalent to the GETRF and GETRI LAPACK routines.</remarks>
        [SecuritySafeCritical]
        public override void LUInverse(float[] a, int order)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (a.Length != order*order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(a));
            }

            Solver(SafeNativeMethods.s_lu_inverse(_solverHandle, _blasHandle, order, a));
        }

        /// <summary>
        /// Computes the inverse of a previously factored matrix.
        /// </summary>
        /// <param name="a">The LU factored N by N matrix.  Contains the inverse On exit.</param>
        /// <param name="order">The order of the square matrix <paramref name="a"/>.</param>
        /// <param name="ipiv">The pivot indices of <paramref name="a"/>.</param>
        /// <remarks>This is equivalent to the GETRI LAPACK routine.</remarks>
        [SecuritySafeCritical]
        public override void LUInverseFactored(float[] a, int order, int[] ipiv)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (ipiv == null)
            {
                throw new ArgumentNullException(nameof(ipiv));
            }

            if (a.Length != order*order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(a));
            }

            if (ipiv.Length != order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(ipiv));
            }

            BLAS(SafeNativeMethods.s_lu_inverse_factored(_blasHandle, order, a, ipiv));
        }

        /// <summary>
        /// Solves A*X=B for X using LU factorization.
        /// </summary>
        /// <param name="columnsOfB">The number of columns of B.</param>
        /// <param name="a">The square matrix A.</param>
        /// <param name="order">The order of the square matrix <paramref name="a"/>.</param>
        /// <param name="b">On entry the B matrix; on exit the X matrix.</param>
        /// <remarks>This is equivalent to the GETRF and GETRS LAPACK routines.</remarks>
        [SecuritySafeCritical]
        public override void LUSolve(int columnsOfB, float[] a, int order, float[] b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (a.Length != order*order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(a));
            }

            if (b.Length != columnsOfB*order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(b));
            }

            if (ReferenceEquals(a, b))
            {
                throw new ArgumentException(Resources.ArgumentReferenceDifferent);
            }

            Solver(SafeNativeMethods.s_lu_solve(_solverHandle, order, columnsOfB, a, b));
        }

        /// <summary>
        /// Solves A*X=B for X using a previously factored A matrix.
        /// </summary>
        /// <param name="columnsOfB">The number of columns of B.</param>
        /// <param name="a">The factored A matrix.</param>
        /// <param name="order">The order of the square matrix <paramref name="a"/>.</param>
        /// <param name="ipiv">The pivot indices of <paramref name="a"/>.</param>
        /// <param name="b">On entry the B matrix; on exit the X matrix.</param>
        /// <remarks>This is equivalent to the GETRS LAPACK routine.</remarks>
        [SecuritySafeCritical]
        public override void LUSolveFactored(int columnsOfB, float[] a, int order, int[] ipiv, float[] b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (ipiv == null)
            {
                throw new ArgumentNullException(nameof(ipiv));
            }

            if (a.Length != order*order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(a));
            }

            if (ipiv.Length != order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(ipiv));
            }

            if (b.Length != columnsOfB*order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(b));
            }

            if (ReferenceEquals(a, b))
            {
                throw new ArgumentException(Resources.ArgumentReferenceDifferent);
            }

            Solver(SafeNativeMethods.s_lu_solve_factored(_solverHandle, order, columnsOfB, a, ipiv, b));
        }

        /// <summary>
        /// Computes the Cholesky factorization of A.
        /// </summary>
        /// <param name="a">On entry, a square, positive definite matrix. On exit, the matrix is overwritten with the
        /// the Cholesky factorization.</param>
        /// <param name="order">The number of rows or columns in the matrix.</param>
        /// <remarks>This is equivalent to the POTRF LAPACK routine.</remarks>
        [SecuritySafeCritical]
        public override void CholeskyFactor(float[] a, int order)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (order < 1)
            {
                throw new ArgumentException(Resources.ArgumentMustBePositive, nameof(order));
            }

            if (a.Length != order*order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(a));
            }

            Solver(SafeNativeMethods.s_cholesky_factor(_solverHandle, order, a));
        }

        /// <summary>
        /// Solves A*X=B for X using Cholesky factorization.
        /// </summary>
        /// <param name="a">The square, positive definite matrix A.</param>
        /// <param name="orderA">The number of rows and columns in A.</param>
        /// <param name="b">On entry the B matrix; on exit the X matrix.</param>
        /// <param name="columnsB">The number of columns in the B matrix.</param>
        /// <remarks>This is equivalent to the POTRF add POTRS LAPACK routines.
        /// </remarks>
        [SecuritySafeCritical]
        public override void CholeskySolve(float[] a, int orderA, float[] b, int columnsB)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            if (b.Length != orderA*columnsB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(b));
            }

            if (ReferenceEquals(a, b))
            {
                throw new ArgumentException(Resources.ArgumentReferenceDifferent);
            }

            Solver(SafeNativeMethods.s_cholesky_solve(_solverHandle, orderA, columnsB, a, b));
        }

        /// <summary>
        /// Solves A*X=B for X using a previously factored A matrix.
        /// </summary>
        /// <param name="a">The square, positive definite matrix A.</param>
        /// <param name="orderA">The number of rows and columns in A.</param>
        /// <param name="b">On entry the B matrix; on exit the X matrix.</param>
        /// <param name="columnsB">The number of columns in the B matrix.</param>
        /// <remarks>This is equivalent to the POTRS LAPACK routine.</remarks>
        [SecuritySafeCritical]
        public override void CholeskySolveFactored(float[] a, int orderA, float[] b, int columnsB)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            if (b.Length != orderA*columnsB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(b));
            }

            if (ReferenceEquals(a, b))
            {
                throw new ArgumentException(Resources.ArgumentReferenceDifferent);
            }

            Solver(SafeNativeMethods.s_cholesky_solve_factored(_solverHandle, orderA, columnsB, a, b));
        }

        /// <summary>
        /// Solves A*X=B for X using the singular value decomposition of A.
        /// </summary>
        /// <param name="a">On entry, the M by N matrix to decompose.</param>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="b">The B matrix.</param>
        /// <param name="columnsB">The number of columns of B.</param>
        /// <param name="x">On exit, the solution matrix.</param>
        public override void SvdSolve(float[] a, int rowsA, int columnsA, float[] b, int columnsB, float[] x)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (b.Length != rowsA*columnsB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(b));
            }

            if (x.Length != columnsA*columnsB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(b));
            }

            var s = new float[Math.Min(rowsA, columnsA)];
            var u = new float[rowsA*rowsA];
            var vt = new float[columnsA*columnsA];

            var clone = new float[a.Length];
            a.Copy(clone);
            SingularValueDecomposition(true, clone, rowsA, columnsA, s, u, vt);
            SvdSolveFactored(rowsA, columnsA, s, u, vt, b, columnsB, x);
        }

        /// <summary>
        /// Computes the singular value decomposition of A.
        /// </summary>
        /// <param name="computeVectors">Compute the singular U and VT vectors or not.</param>
        /// <param name="a">On entry, the M by N matrix to decompose. On exit, A may be overwritten.</param>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="s">The singular values of A in ascending value.</param>
        /// <param name="u">If <paramref name="computeVectors"/> is <c>true</c>, on exit U contains the left
        /// singular vectors.</param>
        /// <param name="vt">If <paramref name="computeVectors"/> is <c>true</c>, on exit VT contains the transposed
        /// right singular vectors.</param>
        /// <remarks>This is equivalent to the GESVD LAPACK routine.</remarks>
        [SecuritySafeCritical]
        public override void SingularValueDecomposition(bool computeVectors, float[] a, int rowsA, int columnsA, float[] s, float[] u, float[] vt)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            if (u == null)
            {
                throw new ArgumentNullException(nameof(u));
            }

            if (vt == null)
            {
                throw new ArgumentNullException(nameof(vt));
            }

            if (u.Length != rowsA*rowsA)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(u));
            }

            if (vt.Length != columnsA*columnsA)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(vt));
            }

            if (s.Length != Math.Min(rowsA, columnsA))
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(s));
            }

            if (columnsA > rowsA || !computeVectors) // see remarks http://docs.nvidia.com/cuda/cusolver/index.html#cuds-lt-t-gt-gesvd
                base.SingularValueDecomposition(computeVectors, a, rowsA, columnsA, s, u, vt);
            else Solver(SafeNativeMethods.s_svd_factor(_solverHandle, computeVectors, rowsA, columnsA, a, s, u, vt));
        }
    }
}

#endif
