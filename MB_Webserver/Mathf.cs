namespace MusicBeePlugin
{
    public static class Mathf
    {
        public static float Range(float value, float min, float max)
        {
            if (value >= min && value <= max)
            {
                return value;
            }
            if (value > max)
            {
                return max;
            }
            else if (value < min)
            {
                return min;
            }

            return 0;
        }
    }
}
