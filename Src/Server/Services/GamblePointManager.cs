using BTB.Application.Common.Exceptions;
using BTB.Application.Common.Interfaces;
using BTB.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace BTB.Server.Services
{
    public class GamblePointManager : IGamblePointManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBTBDbContext _context;

        public GamblePointManager(UserManager<ApplicationUser> userManager, IBTBDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// This function is used when new user is registered.
        /// Adding new gamble gamblePoints with free gamblePoints to database.
        /// </summary>
        /// <param name="amount">Number of gamblePoints to initialize.</param>
        /// <param name="userName">Used to find user by name.</param>
        public async Task<Unit> InitGamblePoints(string userName, decimal amount, CancellationToken cancellationToken)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(userName);

            if(user == null)
            {
                throw new BadRequestException("Unable to find user.");
            }

            _context.GamblePoints.Add(new GamblePoint() { UserId = user.Id, FreePoints = amount });
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        /// <summary>
        /// Function used in cron to update all gamble points daily. Adding amount of free gamblePoints.
        /// </summary>
        /// <param name="amount">Number of gamble points to add.</param>
        public async Task<Unit> AddValueToAllGamblePoints(decimal amount, CancellationToken cancellationToken)
        {
            var gamblePoints = _context.GamblePoints;

            foreach(var p in gamblePoints)
            {
                p.FreePoints += amount;
            };

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        /// <summary>
        /// Add or substract amount from free gamblePoints.
        /// </summary>
        /// <param name="userId">Used to find user gamblePoints.</param>
        /// <param name="amount">Number of gamblePoints to add or substract.</param>
        public async Task<Unit> ChangeFreePointsAmount(string userId, decimal amount, CancellationToken cancellationToken)
        {
            GamblePoint userPoints = _context.GamblePoints.Find(userId);

            if(userPoints == null)
            {
                throw new BadRequestException($"Unable to find gamblePoints for user with id {userId}.");
            }

            userPoints.FreePoints += amount;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        /// <summary>
        /// Add or substract amount from sealed gamblePoints.
        /// </summary>
        /// <param name="userId">Used for find user gamblePoints.</param>
        /// <param name="amount">Number of gamblePoints to add or substract.</param>
        public async Task<Unit> ChangeSealedPointsAmount(string userId, decimal amount, CancellationToken cancellationToken)
        {
            GamblePoint userPoints = _context.GamblePoints.Find(userId);

            if (userPoints == null)
            {
                throw new BadRequestException($"Unable to find gamblePoints for user with id {userId}.");
            }

            userPoints.SealedPoints += amount;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }


        /// <summary>
        /// Transfer amount from free gamblePoints to sealed gamblePoints.
        /// </summary>
        /// <param name="userId">Used for find user gamblePoints.</param>
        /// <param name="amount">Number of gamblePoints to transfer.</param>
        public async Task<Unit> SealGamblePoints(string userId, decimal amount, CancellationToken cancellationToken)
        {
            GamblePoint userPoints = _context.GamblePoints.Find(userId);

            if (userPoints == null)
            {
                throw new BadRequestException($"Unable to find gamblePoints for user with id {userId}.");
            }

            userPoints.FreePoints -= amount;
            userPoints.SealedPoints += amount;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        /// <summary>
        /// Transfer amount from sealed gamblePoints to free gamblePoints.
        /// </summary>
        /// <param name="userId">Used for find user gamblePoints.</param>
        /// <param name="amount">Number of gamblePoints to transfer.</param>
        public async Task<Unit> UnsealGamblePoints(string userId, decimal amount, CancellationToken cancellationToken)
        {
            GamblePoint userPoints = _context.GamblePoints.Find(userId);

            if (userPoints == null)
            {
                throw new BadRequestException($"Unable to find gamblePoints for user with id {userId}.");
            }

            userPoints.SealedPoints -= amount;
            userPoints.FreePoints += amount;
            
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        /// <summary>
        /// Used to get number of user free gamblePoints
        /// </summary>
        /// <param name="userId">Used for find user gamblePoints.</param>
        public decimal GetNumberOfFreePoints(string userId)
        {
            GamblePoint userPoints = _context.GamblePoints.Find(userId);

            if (userPoints == null)
            {
                throw new BadRequestException($"Unable to find gamblePoints for user with id {userId}.");
            }

            return userPoints.FreePoints;
        }
    }
}
