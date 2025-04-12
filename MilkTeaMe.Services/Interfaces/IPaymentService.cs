﻿using MilkTeaMe.Services.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentModel> GetPaymentInfo(int paymentId); 
    }
}
