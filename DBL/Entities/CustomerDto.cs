using DBL.Enums;

namespace DBL.Entities
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public CustomerSegment Segment { get; set; }
    }
}
