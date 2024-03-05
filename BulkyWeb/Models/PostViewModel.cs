using System;
using System.Collections.Generic;

namespace BulkyWeb.Models;


public class PostViewModel
{

    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public int AnswerCount { get; set; }
    public int VoteCount { get; set; }
    public int PostTypeId { get; set; }
    public String? OwnerDisplayName { get; set; }
    public int? OwnerReputation { get; set; }

    public String? Badges { get; set; }
}
