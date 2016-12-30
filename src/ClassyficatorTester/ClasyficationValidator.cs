﻿using System.Collections.Generic;
using Contract.Model;
using Logic.Classifiers;

namespace ClassyficatorTester
{
    public static class ClasyficationValidator
    {
        public static ConfusionMatrix Validate(List<ImageFeatures> train, List<ImageFeatures> validation)
        {
            var smvClassifier = SMVClassifier.Create(train);

            var confusionMatrix = new ConfusionMatrix();
            foreach (var validationObservation in validation)
            {
                var predict = smvClassifier.Predict(validationObservation);
                confusionMatrix.AddVote(actual: validationObservation.IsOccupied, predicted: predict);
            }
            return confusionMatrix;
        }

        public static ConfusionMatrix CrossValidation(List<ImageFeatures> observations, int iterations,
            double splitPercent)
        {
            var summaryConfusionMatrix = new ConfusionMatrix();
            for (int i = 0; i < iterations; i++)
            {
                var tuple = observations.Shuffle().Split(splitPercent);
                var iterationConfusionMatrix = Validate(tuple.Item1, tuple.Item2);
                summaryConfusionMatrix += iterationConfusionMatrix;
            }
            return summaryConfusionMatrix;
        }
    }
}