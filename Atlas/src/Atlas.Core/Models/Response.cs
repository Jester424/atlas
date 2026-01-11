using System;
using System.Collections.Generic;
using System.Text;

namespace Atlas.Core.Models
{
    public class Response
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public Guid MediaItemId {  get; init; }

        public int Rating { get; init; }
    }
}
