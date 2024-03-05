using System;
using System.Collections.Generic;

namespace BulkyWeb.Models;

public partial class Vote
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public int? UserId { get; set; }

    public int? BountyAmount { get; set; }

    public int VoteTypeId { get; set; }

    public DateTime CreationDate { get; set; }
}
