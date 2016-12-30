﻿using System;
using System.Collections.Generic;
using System.Linq;
using Contract.Model;
using OpenCvSharp;

namespace Logic.utils
{
    public static class ImageFeaturesExtensions
    {
        public static ImageFeatures CalculateFeatures(this Mat mat, Contour contour, bool isOccupied)
        {
            var hsvColorStats = Gu.GetHSVColorStats(contour, mat);

            var features = new ImageFeatures
            {
                IsOccupied = isOccupied,
                // counted edges and saturated pixels
                EdgePixels = Gu.CountEdgePixels(contour, mat),
                SaturatedPixels = Gu.CountSaturationPixels(contour, mat),
                MaskPixels = Gu.CountMaskArea(contour, mat),
                //hsv stats
                SaturationMean = hsvColorStats.Item1.Item1,
                SaturationStddev = hsvColorStats.Item1.Item2,
                ValueMean = hsvColorStats.Item2.Item1,
                ValueStddev = hsvColorStats.Item2.Item2
            };

            return features;
        }

        public static Mat ToPredictionMat(this ImageFeatures features)
        {
            float[] sample =
            {
                features.SaturatedPixelsRatio,
                features.EdgePixelsRatio,
                features.SaturationMean,
                features.SaturationStddev,
                features.ValueMean,
                features.ValueStddev
            };
            return new Mat(1, 6, MatType.CV_32FC1, sample);
        }

        public static Mat ToTrainingMat(this List<ImageFeatures> features)
        {
            var count = features.Count;

            float[,] sample = new float[count, 6];
            for (int i = 0; i < count; i++)
            {
                sample[i, 0] = features[i].SaturatedPixelsRatio;
                sample[i, 1] = features[i].EdgePixelsRatio;
                sample[i, 2] = features[i].SaturationMean;
                sample[i, 3] = features[i].SaturationStddev;
                sample[i, 4] = features[i].ValueMean;
                sample[i, 5] = features[i].ValueStddev;
            }

            return new Mat(count, 6, MatType.CV_32FC1, sample);
        }

        public static Mat ToResponseMat(this List<ImageFeatures> features)
        {
            int[] responses = features.Select(f => Convert.ToInt32(f.IsOccupied)).ToArray();

            return new Mat(responses.Length, 1, MatType.CV_32SC1, responses);
        }
    }
}