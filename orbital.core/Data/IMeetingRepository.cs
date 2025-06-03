﻿using orbital.core;

namespace orbital.core.Data
{
    public interface IMeetingRepository
    {
        Task<IEnumerable<Meeting>> GetMeetingsAsync();

        Task<Meeting> GetMeetingByIdAsync(string id);
    }
}