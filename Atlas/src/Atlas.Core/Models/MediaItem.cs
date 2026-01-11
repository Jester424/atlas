using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Atlas.Core.Models
{
    public class MediaItem
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public string Title { get; init; } = string.Empty;

        public int? ReleaseYear { get; init; }

        public MediaType Type { get; init; }
    }
}
