using BudgetTracker.Application.Service;
using BudgetTracker.Core.Domain.Entities;
using BudgetTracker.Core.Domain.Models.Request.Account;
using BudgetTracker.Core.Domain.Repository;
using BudgetTracker.Core.Domain.UnitOfWork;
using BudgetTracker.Core.Domain.Utils;
using BudgetTracker.Core.Infrastructure.Repository;
using BudgetTracker.Core.Infrastructure.Services;
using BudgetTracker.Infrastructure.Persistence.Repository;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Test
{
    public class AuthServiceTest
    {
        private Mock<IAccountRepository> _accountRepository;
        private Mock<IResetPasswordRepository> _resetPasswordRepository;
        private Mock<IPasswordHelper> _passwordHelper;
        private Mock<IEmailService> _emailService;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IConfiguration> _configuration;
        private AuthenticationAppService _authService;
        [SetUp]
        public void Setup()
        {
            _accountRepository = new Mock<IAccountRepository>();
            _resetPasswordRepository = new Mock<IResetPasswordRepository>();
            _passwordHelper = new Mock<IPasswordHelper>();
            _emailService = new Mock<IEmailService>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _configuration = new Mock<IConfiguration>();

            _authService = new AuthenticationAppService(_accountRepository.Object, _resetPasswordRepository.Object, _passwordHelper.Object, _emailService.Object, _unitOfWork.Object, _configuration.Object);
        }

        [Test]
        public async Task SignUp()
        {
            var request = new CreateAccountRequestDto() 
            {
                Email = "xpto@gmail",
                FirstName = "xpto",
                LastName = "xpto",
                Password = "xpto@assaLddcc55"
            };
            
            var account = new Account()
            {
                Id = 1,
                Email = "xpto@gmail",
                FirstName = "xpto",
                LastName = "xpto",
                Password = "xpto@assaLddcc55",
                EmailVerificationToken = "token",
                EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24),
            };

            _accountRepository.Setup(a => a.AddAsync(It.IsAny<Account>())).Returns(Task.FromResult(account));
            _passwordHelper.Setup(p => p.EncryptUrl(account.Id.ToString())).Returns("hash");
            var service = await _authService.SignUpAsync(request);
            Assert.That(service, Is.EqualTo("We send a verification email for you"));
        }

        [Test]
        public async Task VerifyTokenAsync()
        {
            var account = new Account()
            {
                Id = 1,
                Email = "xpto@gmail",
                FirstName = "xpto",
                LastName = "xpto",
                Password = "hash",
                EmailVerificationToken = "xpto",
                VerifyAttempts = 0,
                EmailVerificationTokenExpiry = DateTime.Now.AddHours(4),
                IsActive = false,
                EmailVerified = false,
                VerifiedAt = null,
            };
            _accountRepository.Setup(a => a.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(account);
            _passwordHelper.Setup(x => x.DecryptUrl(It.IsAny<string>())).Returns("1");

            _accountRepository.Setup(a => a.UpdateAsync(account)).Returns(Task.FromResult(true));

            var request = new VerifyTokenRequestDto()
            {
                Id = "xpto@gmail",
                Token = "xpto"
            };
            var service = await _authService.VerifyTokenAsync(request);

            Assert.That(service, Is.EqualTo("Your email has been verified successfully"));
        }
    }
}

