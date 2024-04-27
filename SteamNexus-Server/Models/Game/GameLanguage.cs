﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SteamNexus.Models;

public partial class GameLanguage
{
    [Key]
    [Required]
    public int GameLanguageId { get; set; }

    [Required]
    public int GameId { get; set; }

    [Required]
    public int LanguageId { get; set; }

    [Required]
    public int Support { get; set; } = 0;

    // 導覽屬性

    public virtual Game Game { get; set; }

    public virtual Language Language { get; set; }
}