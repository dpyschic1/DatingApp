namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateOnly dob) // Extend DateTime to calculate age
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = today.Year - dob.Year;

            if(dob > today.AddYears(-age)) age--; // if the birthday has not happened yet take off a year

            return age;
        }
    }
}