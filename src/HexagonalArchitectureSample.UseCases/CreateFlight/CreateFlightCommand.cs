namespace HexagonalArchitectureSample.UseCases.CreateFlight
{
    public class CreateFlightCommand
    {
        public CreateFlightCommand(ushort capacity)
        {
            Capacity = capacity;
        }

        public ushort Capacity { get; }
    }
}
