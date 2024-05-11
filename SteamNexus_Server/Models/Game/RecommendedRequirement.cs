﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SteamNexus_Server.Models;

public partial class RecommendedRequirement
{
    [Key]
    [Required]
    public int RecReqId { get; set; }

    [Required]
    public int GameId { get; set; }
    public virtual Game Game { get; set; }

    [Required]
    public int CPUId { get; set; } = 10000;

    public virtual CPU CPU { get; set; }

    [Required]
    public int GPUId { get; set; } = 10000;

    public virtual GPU GPU { get; set; }

    [Required]
    public int RAM { get; set; } = 4;

    #nullable enable

    [MaxLength(300)]
    public string? OriCpu { get; set; }

    [MaxLength(1000)]
    public string? OriGpu { get; set; }

    [MaxLength(300)]
    public string? OriRam { get; set; }

    [MaxLength(100)]
    public string? OS { get; set; }

    [MaxLength(100)]
    public string? DirectX { get; set; }

    [MaxLength(100)]
    public string? Network { get; set; }

    [MaxLength(100)]
    public string? Storage { get; set; }

    [MaxLength(100)]
    public string? Audio { get; set; }

    [MaxLength(500)]
    public string? Note { get; set; }
}