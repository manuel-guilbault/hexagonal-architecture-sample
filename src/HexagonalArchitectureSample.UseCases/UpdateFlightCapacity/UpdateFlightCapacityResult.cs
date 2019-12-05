namespace HexagonalArchitectureSample.UseCases.UpdateFlightCapacity
{
    public abstract class UpdateFlightCapacityResult
    {
        public class Updated: UpdateFlightCapacityResult
        {
        }

        public class NotFound: UpdateFlightCapacityResult
        {
        }

        public class WouldBeOverbooked: UpdateFlightCapacityResult
        {
        }

        public class Outdated: UpdateFlightCapacityResult
        {
        }
    }
}
