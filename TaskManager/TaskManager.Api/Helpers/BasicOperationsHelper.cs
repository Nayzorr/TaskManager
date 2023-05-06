namespace TaskManager.Api.Helpers
{
    public static class BasicOperationsHelper
    {
        public static bool CheckIntValueValid(int? valueToCheck)
        {
            if (valueToCheck.HasValue && valueToCheck != 0)
            {
                return true;
            }

            return false;
        }
    }
}