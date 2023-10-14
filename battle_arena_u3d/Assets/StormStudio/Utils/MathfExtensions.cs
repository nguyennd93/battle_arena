using UnityEngine;
using System.Collections;
using System.Globalization;
using System;

public static class MathfExtensions
{
    // taken from 
    // http://code.google.com/p/processing/source/browse/trunk/processing/core/src/processing/core/PApplet.java
    // mostly convenience functions. The Trig stuff is a bit pointless, Should remove it at a later date.

    public const float PI2 = Mathf.PI * 2f;

    static public bool FloatEquals(float a, float b, float epsilon = float.Epsilon)
    {
        return Abs(a - b) < epsilon;
    }

    static public float Abs(float n)
    {
        return (n < 0) ? -n : n;
    }

    static public int Abs(int n)
    {
        return (n < 0) ? -n : n;
    }

    static public float Sq(float a)
    {
        return a * a;
    }

    static public int Max(int a, int b)
    {
        return (a > b) ? a : b;
    }

    static public float Max(float a, float b)
    {
        return (a > b) ? a : b;
    }

    static public int Max(int a, int b, int c)
    {
        return (a > b) ? ((a > c) ? a : c) : ((b > c) ? b : c);
    }

    static public float Max(float a, float b, float c)
    {
        return (a > b) ? ((a > c) ? a : c) : ((b > c) ? b : c);
    }

    // NOTE! always use this and not float.parse as float.parse will eat it if the thing being parsed is not a float
    // AND more importantly will not parse correctly in foreign locales where , == .
    public static float parseFloat(object raw)
    {
        if (raw == null) return 0;
        float number = 0f;
        if (float.TryParse(raw as string, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
        {
            return number;
        }
        return 0f;
    }

    public static decimal parseDecimal(object raw)
    {
        if (raw == null) return 0m;
        decimal number = 0m;
        if (decimal.TryParse(raw as string, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
        {
            return number;
        }
        return 0m;
    }

    public static double parseDouble(object raw)
    {
        if (raw == null) return 0;
        double number = 0f;
        if (double.TryParse(raw as string, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
        {
            return number;
        }
        return 0;
    }

    public static long parseLong(object raw)
    {
        if (raw == null) return 0;
        long number = 0;
        if (long.TryParse(raw as string, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
        {
            return number;
        }
        return 0;
    }

    public static int parseInt(object raw)
    {
        if (raw == null) return 0;
        int number = 0;
        if (int.TryParse(raw as string, NumberStyles.Any, CultureInfo.InvariantCulture, out number))
        {
            return number;
        }
        return 0;
    }

    // this is included mostly for completeness, not necessarily because it is math related
    public static bool parseBool(object unknownType)
    {
        if (unknownType == null) return false;
        // should be a string
        if (unknownType.ToString().ToLower() == "yes")
            return true;
        if (unknownType.ToString().ToLower() == "true")
            return true;
        if (unknownType.ToString().ToLower() == "y")
            return true;
        if (unknownType.ToString().ToLower() == "1")
            return true;
        return false;
    }


    // this is also included mostly for completeness, not necessarily because it is math related
    // also this hurts me since it uses reflection and is slow as fuck and if this code had a face I would punch it
#if NETFX_CORE
        public static T parseEnum<T>( string value ) where T : struct
#else
    public static T parseEnum<T>(string value) where T : struct, IConvertible
#endif
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

#if NETFX_CORE
        public static T tryParseEnum<T>(string value, T defaultSetting) where T : struct
#else
    public static T tryParseEnum<T>(string value, T defaultSetting) where T : struct, IConvertible
#endif
    {
        try
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        catch
        {
            return defaultSetting;
        }
    }

    public static Vector2 parseVector2(string raw)
    {
        if (raw == null) return Vector2.zero;
        // raw looks like x,y
        string[] tokens = raw.Split(',');
        if (tokens.Length != 2) return Vector2.zero;
        float x = parseFloat(tokens[0]);
        float y = parseFloat(tokens[1]);
        return new Vector2(x, y);
    }

    public static Vector3 parseVector3(string raw)
    {
        if (raw == null) return Vector3.zero;
        // raw looks like x,y
        string[] tokens = raw.Split(',');
        if (tokens.Length != 3) return Vector3.zero;
        float x = parseFloat(tokens[0]);
        float y = parseFloat(tokens[1]);
        float z = parseFloat(tokens[2]);
        return new Vector3(x, y, z);
    }

    /**
    * Find the maximum value in an array.
    * Throws an ArrayIndexOutOfBoundsException if the array is Mathf. 0.
    * @param list the source array
    * @return The maximum value
    */
    static public int Max(int[] list)
    {
        if (list.Length == 0)
        {
            return 0;
        }
        int max = list[0];
        for (int i = 1; i < list.Length; i++)
        {
            if (list[i] > max)
            {
                max = list[i];
            }
        }
        return max;
    }

    /**
    * Find the maximum value in an array.
    * Throws an ArrayIndexOutOfBoundsException if the array is length 0.
    * @param list the source array
    * @return The maximum value
    */
    static public float Max(float[] list)
    {
        if (list.Length == 0)
        {
            return 0;
        }
        float max = list[0];
        for (int i = 1; i < list.Length; i++)
        {
            if (list[i] > max)
            {
                max = list[i];
            }
        }
        return max;
    }

    static public int Min(int a, int b)
    {
        return (a < b) ? a : b;
    }

    static public float Min(float a, float b)
    {
        return (a < b) ? a : b;
    }

    static public int Min(int a, int b, int c)
    {
        return (a < b) ? ((a < c) ? a : c) : ((b < c) ? b : c);
    }

    static public float Min(float a, float b, float c)
    {
        return (a < b) ? ((a < c) ? a : c) : ((b < c) ? b : c);
    }

    /**
    * Find the minimum value in an array.
    * Throws an ArrayIndexOutOfBoundsException if the array is length 0.
    * @param list the source array
    * @return The minimum value
    */
    static public int Min(int[] list)
    {
        if (list.Length == 0)
        {
            return 0;
        }
        int min = list[0];
        for (int i = 1; i < list.Length; i++)
        {
            if (list[i] < min)
            {
                min = list[i];
            }
        }
        return min;
    }

    /**
    * Find the minimum value in an array.
    * Throws an ArrayIndexOutOfBoundsException if the array is length 0.
    * @param list the source array
    * @return The minimum value
    */
    static public float Min(float[] list)
    {
        if (list.Length == 0)
        {
            return 0;
        }
        float min = list[0];
        for (int i = 1; i < list.Length; i++)
        {
            if (list[i] < min)
            {
                min = list[i];
            }
        }
        return min;
    }

    static public float Wrap(float value, float length)
    {
        return value % length;
    }

    static public double Pow(double num, int exp)
    {
        double result = 1.0;

        while (exp > 0)
        {
            if (exp % 2 == 1)
            {
                result *= num;
            }

            exp >>= 1;
            num *= num;
        }

        return result;
    }

    static public int Constrain(int amt, int low, int high)
    {
        return (amt < low) ? low : ((amt > high) ? high : amt);
    }

    static public float Constrain(float amt, float low, float high)
    {
        return (amt < low) ? low : ((amt > high) ? high : amt);
    }

    static public float Degrees(float radians)
    {
        return radians * Mathf.Rad2Deg;
    }

    static public float Radians(float degrees)
    {
        return degrees * Mathf.Deg2Rad;
    }

    static public float Mag(float a, float b)
    {
        return (float)Mathf.Sqrt(a * a + b * b);
    }

    static public float Mag(float a, float b, float c)
    {
        return (float)Mathf.Sqrt(a * a + b * b + c * c);
    }

    static public float Dist(float x1, float x2)
    {
        return Mathf.Abs(x2 - x1);// Mathf.Sqrt (Sq (x2 - x1));
    }

    static public float Dist(float x1, float y1, float x2, float y2)
    {
        return Mathf.Sqrt(Sq(x2 - x1) + Sq(y2 - y1));
    }

    static public float Dist(float x1, float y1, float z1, float x2, float y2, float z2)
    {
        return Mathf.Sqrt(Sq(x2 - x1) + Sq(y2 - y1) + Sq(z2 - z1));
    }

    static public float DistSq(float x1, float x2)
    {
        return Sq(x2 - x1);
    }

    static public float DistSq(float x1, float y1, float x2, float y2)
    {
        return Sq(x2 - x1) + Sq(y2 - y1);
    }

    static public float DistSq(float x1, float y1, float z1, float x2, float y2, float z2)
    {
        return Sq(x2 - x1) + Sq(y2 - y1) + Sq(z2 - z1);
    }
    static public float Lerp(float start, float stop, float amt)
    {
        return start + (stop - start) * amt;
    }

    /**
    * Normalize a value to exist between 0 and 1 (inclusive).
    * Mathematically the opposite of lerp(), figures out what proportion
    * a particular value is relative to start and stop coordinates.
    */
    static public float Norm(float value, float start, float stop)
    {
        return (value - start) / (stop - start);
    }

    /**
    * Convenience function to map a variable from one coordinate space
    * to another. Equivalent to unlerp() followed by lerp().
    */
    static public float Map(float value, float istart, float istop, float ostart, float ostop)
    {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }

    static public Vector3 Map(Vector3 value, Vector3 istart, Vector3 istop, Vector3 ostart, Vector3 ostop)
    {
        Vector3 result;
        result.x = ostart.x + ((ostop.x - ostart.x) * ((value.x - istart.x) / (istop.x - istart.x)));
        result.y = ostart.y + ((ostop.y - ostart.y) * ((value.y - istart.y) / (istop.y - istart.y)));
        result.z = ostart.z + ((ostop.z - ostart.z) * ((value.z - istart.z) / (istop.z - istart.z)));

        return result;
    }

    static public Vector2 GetUnitOnCircle(float angleDegrees, float radius)
    {
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        return new Vector2(radius * Mathf.Cos(angleRadians), radius * Mathf.Sin(angleRadians));
    }

    public static bool Approximately(float a, float b)
    {
        float c = (b - a) < 0 ? -(b - a) : (b - a);
        a = (a < 0) ? -a : a;
        b = (b < 0) ? -b : b;


        a = 1E-06f * ((a <= b) ? b : a);

        b = (a <= 1.121039E-44f) ? 1.121039E-44f : a;

        return c < b;
    }

    public static bool Approximately(Vector2 a, Vector2 b)
    {
        return MathfExtensions.Approximately(a.x, b.x) && MathfExtensions.Approximately(a.y, b.y);
    }

    public static bool Approximately(Vector3 a, Vector3 b)
    {
        return MathfExtensions.Approximately(a.x, b.x) && MathfExtensions.Approximately(a.y, b.y) && MathfExtensions.Approximately(a.z, b.z);
    }

    public static bool Approximately(Vector4 a, Vector4 b)
    {
        return MathfExtensions.Approximately(a.x, b.x) && MathfExtensions.Approximately(a.y, b.y) && MathfExtensions.Approximately(a.z, b.z) && MathfExtensions.Approximately(a.w, b.w);
    }

    public static bool Approximately(Color a, Color b)
    {
        return MathfExtensions.Approximately(a.r, b.r) && MathfExtensions.Approximately(a.g, b.g) && MathfExtensions.Approximately(a.b, b.b) && MathfExtensions.Approximately(a.a, b.a);
    }

    public static bool Approximately(Quaternion a, Quaternion b)
    {
        return MathfExtensions.Approximately(a.x, b.x) && MathfExtensions.Approximately(a.y, b.y) && MathfExtensions.Approximately(a.z, b.z) && MathfExtensions.Approximately(a.w, b.w);
    }
}
