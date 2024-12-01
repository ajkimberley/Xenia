using Xenia.Application.Bookings;
using Xenia.Common;

namespace Xenia.WebApi.RequestResponse;

public record BookingResponseDto(BookingDto BookingDto, List<LinkDto> Links);
