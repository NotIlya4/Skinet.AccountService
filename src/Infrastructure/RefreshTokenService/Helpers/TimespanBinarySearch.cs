namespace Infrastructure.RefreshTokenService.Helpers;

public class TimespanBinarySearch
{
    public static int DeleteCount(List<DateTime> sortedDates, TimeSpan expireIn)
    {
        int left = -1;
        int right = sortedDates.Count - 1;

        while (left < right)
        {
            int mid;
            if (right - left == 1)
            {
                mid = right;
            }
            else
            {
                mid = (left + right) / 2;
            }
            
            TimeSpan issueDelta = DateTime.UtcNow - sortedDates[mid];

            bool isGoingToBeDeleted = issueDelta >= expireIn;

            if (isGoingToBeDeleted)
            {
                left = mid;
            }
            else
            {
                right = Math.Clamp(mid - 1, left, right);
            }
        }

        return left + 1;
    }
}