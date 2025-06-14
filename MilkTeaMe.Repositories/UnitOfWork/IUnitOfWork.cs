﻿using MilkTeaMe.Repositories.Implementations;
using MilkTeaMe.Repositories.Interfaces;
using MilkTeaMe.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Category> CategoryRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        IOrderRepository OrderRepository { get; }
        IGenericRepository<OrderDetail> OrderDetailRepository { get; }
        IGenericRepository<Payment> PaymentRepository { get; }
        IGenericRepository<PaymentMethod> PaymentMethodRepository { get; }
        IProductRepository ProductRepository { get; }
        IGenericRepository<ProductCombo> ProductComboRepository { get; }
        IGenericRepository<ProductSize> ProductSizeRepository { get; }
        IGenericRepository<Size> SizeRepository { get; }
        IGenericRepository<PasswordResetToken> PasswordResetTokenRepository { get; }
		Task SaveChangesAsync();
    }
}
