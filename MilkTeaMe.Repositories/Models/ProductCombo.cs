﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MilkTeaMe.Repositories.Models;

public partial class ProductCombo
{
    public int ComboId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public int? ProductSizeId { get; set; }

    public virtual Product Combo { get; set; }

    public virtual Product Product { get; set; }

    public virtual ProductSize ProductSize { get; set; }
}