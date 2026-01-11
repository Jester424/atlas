namespace Atlas.Core.Scoring
{
    public static class FamiliarityScoreCalculator
    {
        private const int MaxRating = 4;
        private const double ScaleFactor = 25.0;
        private const double NeutralScore = 50.0;
        private const int DefaultNeutralPriorStrength = 10;

        public static double CalculateScore(IEnumerable<int> ratings0to4, int neutralPriorStrength = DefaultNeutralPriorStrength)
        {
            if (ratings0to4 is null)
                throw new ArgumentNullException(nameof(ratings0to4));

            if (neutralPriorStrength < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(neutralPriorStrength),
                    "Neutral prior strength must be >= 0.");

            int n = 0;
            double sum = 0;

            foreach (var r in ratings0to4)
            {
                if (r < 0 || r > MaxRating)
                    throw new ArgumentOutOfRangeException(
                        nameof(ratings0to4),
                        $"Ratings must be between 0 and {MaxRating}.");

                sum += r * ScaleFactor;
                n++;
            }

            if (n == 0) return NeutralScore;

            return (sum + (neutralPriorStrength * NeutralScore)) /
                (n + neutralPriorStrength);
        }
    }
}
