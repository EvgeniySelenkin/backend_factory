public class NumericShiftDetector
{
    public int GetShiftPosition(int[] initialArray)
    {
        int left = 0, right = initialArray.Length - 1;
        if (initialArray[left] < initialArray[right])
        {
            return 0;
        }
        else
        {
            while (right - left > 1)
            {
                if (initialArray[left] > initialArray[(int)((right + left) / 2)])
                    right = (int)(right + left) / 2;
                else if (initialArray[left] < initialArray[(int)((right + left) / 2)])
                    left = (int)(right + left) / 2;
            }
            return left + 1;
        }

    }
}
