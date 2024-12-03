using Xenia.Bookings.Domain.Availabilities;

namespace Xenia.Bookings.Persistence.Repositories;

public class AvailabilityRepository(BookingContext context) : GenericRepository<Availability>(context), IAvailabilityRepository;