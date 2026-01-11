using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Core.Scoring
{
    public static class FamiliarityScoreCalculator
    {
        public static double CalculateScore(IEnumerable<int> ratings0to4, int k = 10)
        {
            if (ratings0to4 is null) throw new ArgumentNullException(nameof(ratings0to4));
            if (k < 0) throw new ArgumentOutOfRangeException(nameof(k), "k must be >= 0.");

            int n = 0;
            double sum = 0;

            foreach (var r in ratings0to4)
            {
                if (r < 0 || r > 4)
                    throw new ArgumentOutOfRangeException(nameof(r), "Ratings must be between 0 and 4.");

                sum += r * 25.0;
                n++;
            }

            if (n == 0) return 50.0;

            return (sum + (k * 50.0)) / (n + k);
        }
    }
}
