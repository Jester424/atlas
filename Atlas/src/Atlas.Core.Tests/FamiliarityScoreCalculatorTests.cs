using Atlas.Core.Scoring;

namespace Atlas.Core.Tests.Scoring
{
    public class FamiliarityScoreCalculatorTests
    {
        [Fact]
        public void CalculateScore_NullRatings_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                FamiliarityScoreCalculator.CalculateScore(null!));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        public void CalculateScore_NegativeNeutralPriorStrength_ThrowsArgumentOutOfRangeException(int neutralPriorStrength)
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
                FamiliarityScoreCalculator.CalculateScore(new[] { 0, 1, 2 }, neutralPriorStrength));

            Assert.Equal("neutralPriorStrength", ex.ParamName);
        }

        [Fact]
        public void CalculateScore_EmptyRatings_ReturnsNeutralScore()
        {
            var score = FamiliarityScoreCalculator.CalculateScore(Array.Empty<int>());
            Assert.Equal(50.0, score, precision: 10);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(5)]
        [InlineData(100)]
        public void CalculateScore_RatingOutOfRange_ThrowsArgumentOutOfRangeException(int badRating)
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
                FamiliarityScoreCalculator.CalculateScore(new[] { 0, badRating, 4 }));

            // Important: you threw with nameof(ratings0to4), so ParamName should match that.
            Assert.Equal("ratings0to4", ex.ParamName);
        }

        [Fact]
        public void CalculateScore_SingleRating_NoPrior_ReturnsScaledValue()
        {
            // rating 4 -> 4 * 25 = 100
            var score = FamiliarityScoreCalculator.CalculateScore(new[] { 4 }, neutralPriorStrength: 0);
            Assert.Equal(100.0, score, precision: 10);
        }

        [Fact]
        public void CalculateScore_MultipleRatings_NoPrior_ReturnsAverageScaledValue()
        {
            // ratings: 0, 4 -> scaled: 0, 100 -> average 50
            var score = FamiliarityScoreCalculator.CalculateScore(new[] { 0, 4 }, neutralPriorStrength: 0);
            Assert.Equal(50.0, score, precision: 10);
        }

        [Fact]
        public void CalculateScore_WithPrior_PullsTowardNeutral()
        {
            // Without prior: [4] => 100
            // With strong prior: should be closer to 50 than 100
            var noPrior = FamiliarityScoreCalculator.CalculateScore(new[] { 4 }, neutralPriorStrength: 0);
            var withPrior = FamiliarityScoreCalculator.CalculateScore(new[] { 4 }, neutralPriorStrength: 10);

            Assert.Equal(100.0, noPrior, precision: 10);
            Assert.True(withPrior < 100.0);
            Assert.True(withPrior > 50.0);
        }

        [Fact]
        public void CalculateScore_WithPrior_ComputesExpectedWeightedMean()
        {
            // ratings: 4, 4 => sum scaled = 200, n = 2
            // priorStrength = 10, neutral = 50
            // expected = (200 + 10*50) / (2 + 10) = (200 + 500)/12 = 700/12 = 58.3333333333
            var score = FamiliarityScoreCalculator.CalculateScore(new[] { 4, 4 }, neutralPriorStrength: 10);

            Assert.Equal(700.0 / 12.0, score, precision: 10);
        }

        [Fact]
        public void CalculateScore_LazyEnumerable_IsEnumeratedOnceAndWorks()
        {
            // This is mostly a “sanity” check that IEnumerable inputs work fine.
            var ratings = Enumerable.Range(0, 5); // 0..4
            var score = FamiliarityScoreCalculator.CalculateScore(ratings, neutralPriorStrength: 0);

            // scaled values: 0,25,50,75,100 average = 50
            Assert.Equal(50.0, score, precision: 10);
        }
    }
}
