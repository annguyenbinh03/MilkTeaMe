﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MilkTeaMe.Repositories.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public int? SizeId { get; set; }

    public int? ParentId { get; set; }

    public virtual ICollection<OrderDetail> InverseParent { get; set; } = new List<OrderDetail>();

    public virtual Order Order { get; set; }

    public virtual OrderDetail Parent { get; set; }

    public virtual Product Product { get; set; }

    public virtual Size Size { get; set; }
}