﻿using Microsoft.EntityFrameworkCore;
using MilkTeaMe.Repositories.DbContexts;
using MilkTeaMe.Repositories.Implementations;
using MilkTeaMe.Repositories.Interfaces;
using MilkTeaMe.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Repositories.UnitOfWork
{
    public class UnitOfWork(MilkTeaMeDBContext context) : IUnitOfWork
    {
        private readonly DbContext _context = context;
        private IGenericRepository<Category>? _categoryRepository;
        private IGenericRepository<Employee>? _employeeRepository;
        private IGenericRepository<Order>? _orderRepository;
        private IGenericRepository<OrderDetail>? _orderDetailRepository;
        private IGenericRepository<Payment>? _paymentRepository;
        private IGenericRepository<PaymentMethod>? _paymentMethodRepository;
        private IGenericRepository<Product>? _productRepository;
        private IGenericRepository<ProductCombo>? _productComboRepository;
        private IGenericRepository<ProductSize>? _productSizeRepository;
        private IGenericRepository<Size>? _sizeRepository;

        public IGenericRepository<Category> CategoryRepository
        {
            get
            {
                if (this._categoryRepository == null)
                {
                    this._categoryRepository = new GenericRepository<Category>(context);
                }
                return _categoryRepository;
            }
        }

        public IGenericRepository<Employee> EmployeeRepository
        {
            get
            {
                if (this._employeeRepository == null)
                {
                    this._employeeRepository = new GenericRepository<Employee>(context);
                }
                return _employeeRepository;
            }
        }

        public IGenericRepository<Order> OrderRepository
        {
            get
            {

                if (this._orderRepository == null)
                {
                    this._orderRepository = new GenericRepository<Order>(context);
                }
                return _orderRepository;
            }
        }

        public IGenericRepository<OrderDetail> OrderDetailRepository
        {
            get
            {

                if (this._orderDetailRepository == null)
                {
                    this._orderDetailRepository = new GenericRepository<OrderDetail>(context);
                }
                return _orderDetailRepository;
            }
        }

        public IGenericRepository<Payment> PaymentRepository
        {
            get
            {

                if (this._paymentRepository == null)
                {
                    this._paymentRepository = new GenericRepository<Payment>(context);
                }
                return _paymentRepository;
            }
        }

        public IGenericRepository<PaymentMethod> PaymentMethodRepository
        {
            get
            {

                if (this._paymentMethodRepository == null)
                {
                    this._paymentMethodRepository = new GenericRepository<PaymentMethod>(context);
                }
                return _paymentMethodRepository;
            }
        }

        public IGenericRepository<Product> ProductRepository
        {
            get
            {
                if (this._productRepository == null)
                {
                    this._productRepository = new GenericRepository<Product>(context);
                }
                return _productRepository;
            }
        }

        public IGenericRepository<ProductCombo> ProductComboRepository
        {
            get
            {
                if (this._productComboRepository == null)
                {
                    this._productComboRepository = new GenericRepository<ProductCombo>(context);
                }
                return _productComboRepository;
            }
        }

        public IGenericRepository<ProductSize> ProductSizeRepository
        {
            get
            {
                if (this._productSizeRepository == null)
                {
                    this._productSizeRepository = new GenericRepository<ProductSize>(context);
                }
                return _productSizeRepository;
            }
        }

        public IGenericRepository<Size> SizeRepository
        {
            get
            {
                if (this._sizeRepository == null)
                {
                    this._sizeRepository = new GenericRepository<Size>(context);
                }
                return _sizeRepository;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
