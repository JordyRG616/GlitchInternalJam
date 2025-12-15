using NaughtyAttributes;
using UnityEngine;

public static class FractaSkin
{
    public static string BackgroundColor = "#131626";
    public static string FontColor = "#ffe6ea";
    public static string BorderColor = "#4d4d80";
    public static string HighlightColor = "#e6a1cf";
    

    public static GUIStyle box
    {
        get
        {
            var style = new GUIStyle(GUI.skin.box);
            style.normal.background = MakeTex(2, 2, GetColor(BackgroundColor, .3f));
            
            return style;
        }
    }
    
    private static Texture2D MakeTex( int width, int height, Color col )
    {
        Color[] pix = new Color[width * height];
        for( int i = 0; i < pix.Length; ++i )
        {
            pix[ i ] = col;
        }
        Texture2D result = new Texture2D( width, height );
        result.SetPixels( pix );
        result.Apply();
        return result;
    }

    public static Color GetColor(string hex, float withAlpha = 1f)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            color.a = withAlpha;
            return color;
        }
        return Color.clear;
    }
}
