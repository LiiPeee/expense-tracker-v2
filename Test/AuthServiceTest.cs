using ExpenseTrackerV2.Application.Service;
using ExpenseTrackerV2.Core.Domain.Entities;
using ExpenseTrackerV2.Core.Domain.Models.Request.Account;
using ExpenseTrackerV2.Core.Domain.Repository;
using ExpenseTrackerV2.Core.Domain.UnitOfWork;
using ExpenseTrackerV2.Core.Domain.Utils;
using ExpenseTrackerV2.Core.Infrastructure.Repository;
using ExpenseTrackerV2.Core.Infrastructure.Services;
using ExpenseTrackerV2.Infrastructure.Persistence.Repository;
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
                Password = "xpto"
            };
            
            var account = new Account()
            {
                Id = 1,
                Email = "xpto@gmail",
                FirstName = "xpto",
                LastName = "xpto",
                Password = "hash",
                EmailVerificationToken = "token",
                EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24),
            };

            _accountRepository.Setup(a => a.AddAsync(account)).Returns(Task.FromResult(account));

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
                EmailVerificationTokenExpiry = DateTime.Now.AddHours(4),
                IsActive = false,
                EmailVerified = false,
                VerifiedAt = null,
            };
            _accountRepository.Setup(a => a.GetByEmailAsync("xpto@gmail")).Returns(Task.FromResult(account));

            account.EmailVerified = true;
            account.EmailVerificationToken = null;
            account.VerifiedAt = DateTime.Now;
            account.IsActive = true;

            _accountRepository.Setup(a => a.UpdateAsync(account)).Returns(Task.FromResult(true));

            var request = new VerifyTokenRequestDto()
            {
                Email = "xpto@gmail",
                Token = "xpto"
            };
            var service = await _authService.VerifyTokenAsync(request);

            Assert.That(service, Is.EqualTo("Your email has been verified successfully"));
        }
    }
}
