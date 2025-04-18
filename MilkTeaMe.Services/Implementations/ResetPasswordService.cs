using MilkTeaMe.Repositories.Models;
using MilkTeaMe.Repositories.UnitOfWork;
using MilkTeaMe.Services.Interfaces;
using MilkTeaMe.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTeaMe.Services.Implementations
{
	public class ResetPasswordService : IResetPasswordService
	{
		private readonly IUnitOfWork _unitOfWork;

		public ResetPasswordService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task Delete(PasswordResetToken token)
		{
			_unitOfWork.PasswordResetTokenRepository.DeleteAsync(token);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task<PasswordResetToken?> FindOneAsync(string email, string token)
		{
			DateTime now = TimeZoneUtil.GetCurrentTime();
			return await _unitOfWork.PasswordResetTokenRepository.FindOneAsync(t =>
					t.Email == email && t.Token == token && t.ExpiredAt > now);
		}
	}
}
