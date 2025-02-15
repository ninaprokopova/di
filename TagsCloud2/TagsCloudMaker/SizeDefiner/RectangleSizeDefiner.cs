﻿using System.Drawing;

namespace TagsCloud2.TagsCloudMaker.SizeDefiner;

public class RectangleSizeDefiner : ISizeDefiner
{
    private int verticalCount;
    private Random rnd;
    public Dictionary<string, TextOptions> DefineStringSizeAndOrientation(
        Dictionary<string, int> stingAndFontSize, bool withVerticalWords, string fontFamilyName)
    {
        var stringSizeAndOrientation = new Dictionary<string, TextOptions>();
        var bitmap = new Bitmap(50, 50);

        if (withVerticalWords)
        {
            DefineSizeWithVertical(stingAndFontSize, fontFamilyName, bitmap, stringSizeAndOrientation);
        }
        else  DefineSizeOnlyHorizontal(stingAndFontSize, fontFamilyName, bitmap, stringSizeAndOrientation);

        return stringSizeAndOrientation;
    }
    
    private void DefineSizeWithVertical(Dictionary<string, int> stingAndFontSize, 
        string fontFamilyName, 
        Bitmap bitmap,
        Dictionary<string, TextOptions> stringSizeAndOrientation)
    {
        verticalCount = 0;
        rnd = new Random();
        using var graphics = Graphics.FromImage(bitmap);
        foreach (var item in stingAndFontSize)
        {
            var fontSize = item.Value;
            using var font = new Font(fontFamilyName, fontSize);
            var size = graphics.MeasureString(item.Key, font).ToSize();
            var orientation = DefineOrientation();
            if (orientation == WordOrientation.Vertical)
            {
                var newSize = new Size(size.Height, size.Width);
                size = newSize;
            };
            stringSizeAndOrientation.Add(item.Key, new TextOptions(size, orientation, fontSize));
        }
    }

    private WordOrientation DefineOrientation()
    {
        var orientation = WordOrientation.Horizontal;
        var rndNumber = rnd.Next(1, 10);
        var isVertical = rndNumber > 6;
        if (verticalCount >= 10 || !isVertical) return orientation;
        orientation = WordOrientation.Vertical;
        verticalCount += 1;
        return orientation;
    }

    private void DefineSizeOnlyHorizontal(Dictionary<string, int> stingAndFontSize, 
        string fontFamilyName, 
        Bitmap bitmap,
        Dictionary<string, TextOptions> stringSizeAndOrientation)
    {
        using var graphics = Graphics.FromImage(bitmap);
        foreach (var item in stingAndFontSize)
        {
            var fontSize = item.Value;
            using var font = new Font(fontFamilyName, fontSize);
            var size = graphics.MeasureString(item.Key, font);
            stringSizeAndOrientation.Add(item.Key, new TextOptions(size.ToSize(), WordOrientation.Horizontal, fontSize));
        }
    }
}