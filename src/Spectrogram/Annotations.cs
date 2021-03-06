﻿using Spectrogram.Settings;
using System;
using System.Drawing;

namespace Spectrogram
{
    public class Annotations
    {
        public static void drawTicks(Bitmap bmp, FftSettings fftSettings, DisplaySettings displaySettings)
        {
            Graphics gfx = Graphics.FromImage(bmp);

            double tickSpacingSec = displaySettings.tickSpacingSec;
            double tickSpacingHz = displaySettings.tickSpacingHz;

            double horizontalTickSpacing = tickSpacingSec * fftSettings.sampleRate / fftSettings.step;

            double firstFreqTick = fftSettings.FrequencyFromIndex(displaySettings.pixelLower) + tickSpacingHz;
            double lastFreqTick = fftSettings.FrequencyFromIndex(displaySettings.pixelUpper) - tickSpacingHz;

            for (double frequency = 0; frequency < fftSettings.maxFreq; frequency += tickSpacingHz)
            {
                if ((frequency < firstFreqTick) || (frequency > lastFreqTick))
                    continue;
                int yPosition = bmp.Height - (fftSettings.IndexFromFrequency(frequency) - displaySettings.pixelLower);
                Point p1 = new Point(bmp.Width - displaySettings.tickSize, yPosition);
                Point p2 = new Point(bmp.Width, yPosition);
                DrawLineWithShadow(gfx, p1, p2);
                DrawTextWithShadow(gfx, Math.Round(frequency).ToString(), p1, displaySettings.tickFont, displaySettings.sfTicksRight);
            }

            for (double xPx = 0; xPx < bmp.Width; xPx += horizontalTickSpacing)
            {
                Point p1 = new Point((int)xPx, bmp.Height);
                Point p2 = new Point((int)xPx, bmp.Height - displaySettings.tickSize);
                DrawLineWithShadow(gfx, p1, p2);
            }
        }

        static void DrawTextWithShadow(Graphics gfx, string s, Point pt, Font fnt, StringFormat sf)
        {
            for (int dX = -1; dX < 2; dX++)
                for (int dY = -1; dY < 2; dY++)
                    gfx.DrawString(s, fnt, Brushes.Black, pt.X + dX, pt.Y + dY, sf);

            gfx.DrawString(s, fnt, Brushes.White, pt, sf);
        }

        static void DrawLineWithShadow(Graphics gfx, Point p1, Point p2)
        {
            Pen penShadow = new Pen(Brushes.Black, 3);
            Pen penTick = new Pen(Brushes.White, 1);
            penShadow.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            penTick.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            gfx.DrawLine(penShadow, p1, p2);
            gfx.DrawLine(penTick, p1, p2);
        }
    }
}
