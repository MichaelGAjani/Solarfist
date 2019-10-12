using Jund.MathHelper.Numerics.LinearAlgebra;

namespace Jund.MathHelper.Numerics.Optimization.TrustRegion
{
    public interface ITrustRegionSubproblem
    {
        Vector<double> Pstep { get; }
        bool HitBoundary { get; }

        void Solve(IObjectiveModel objective, double radius);
    }
}
