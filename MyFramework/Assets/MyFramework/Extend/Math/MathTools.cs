
namespace MyFramework
{
    public static class MathTools
    {
        public static float Range(this float Target, float min, float max)
        {
            if (Target < min)
            {
                return min;
            }
            if (Target > max)
            {
                return max;
            }
            return Target;
        }
    }
}
