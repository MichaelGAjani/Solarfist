using Jund.MathHelper.Numerics.Optimization.TrustRegion.Subproblems;

namespace Jund.MathHelper.Numerics.Optimization.TrustRegion
{
    public static class TrustRegionSubproblem
    {
        public static ITrustRegionSubproblem DogLeg()
        {
            return new DogLegSubproblem();
        }

        public static ITrustRegionSubproblem NewtonCG()
        {
            return new NewtonCGSubproblem();
        }
    }
}
