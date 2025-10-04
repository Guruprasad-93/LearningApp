using gym_app.Domain.Entities.AppOption;
using gym_app.Domain.Entities;
using gym_app.Domain.Interfaces.RepositoryInterface;
using gym_app.Domain.Model.User;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gym_app.Domain.Model.CommonModel;
using gym_app.Domain.Model.CustomerReview;
using Microsoft.EntityFrameworkCore;

namespace gym_app.Infrastructure.Repository.UserRepository
{
    public class UserRepository : BaseRepository<UserRepository>, IUserRepository
    {
        

        private readonly ApplicationDbContext context;
        public AppOptions appOptions { get; set; }
        public UserRepository(ApplicationDbContext _context, ILogger<UserRepository> _logger, AppOptions _appOptions) : base(_logger)
        {
            context = _context;
            appOptions = _appOptions;

        }

        public async Task<string> UserCreationUpdation(UserDetailsModel userDetailsModel, string webRootPath)
        {
            string Result = "";

            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                logger.LogDebug("UserRepository > UserCreation() started {model}", userDetailsModel);

                var duplicateUser = await context.Users
                    .FirstOrDefaultAsync(u => u.UserName == userDetailsModel.UserName && u.UserId != userDetailsModel.UserId);

                if (duplicateUser != null)
                    return "Exist";

                User? user;
                bool isUpdate = context.Users.Any(u => u.UserId == userDetailsModel.UserId && u.CompanyId == userDetailsModel.CompanyId);

                if (isUpdate)
                {
                    user = await context.Users
                        .FirstOrDefaultAsync(u => u.UserId == userDetailsModel.UserId && u.CompanyId == userDetailsModel.CompanyId);

                    if (user != null)
                    {
                        user.FullName = userDetailsModel.FullName;
                        user.Address = userDetailsModel.Address;
                        user.Email = userDetailsModel.Email;
                        user.Phone = userDetailsModel.Phone;
                        user.Gender = userDetailsModel.Gender;
                        user.DateOfBirth = userDetailsModel.DateOfBirth;
                        user.Password = userDetailsModel.Password;
                        user.RoleId = userDetailsModel.RoleId;
                        user.ModifiedBy = userDetailsModel.UserId;
                        user.ModifiedDate = DateTime.Now;

                        context.Users.Update(user);
                        Result = "U001";
                    }
                }
                else
                {
                    user = new User
                    {
                        UserName = userDetailsModel.UserName,
                        Password = userDetailsModel.Password,
                        FullName = userDetailsModel.FullName,
                        Address = userDetailsModel.Address,
                        Email = userDetailsModel.Email,
                        Phone = userDetailsModel.Phone,
                        Gender = userDetailsModel.Gender,
                        DateOfBirth = userDetailsModel.DateOfBirth,
                        RoleId = userDetailsModel.RoleId,
                        IsActive = true,
                        CreatedBy = userDetailsModel.UserId,
                        CreatedDate = DateTime.Now,
                        CompanyId = 1,
                        IsDeleted = false
                    };

                    context.Users.Add(user);
                    await context.SaveChangesAsync(); // Needed to get user.UserId

                    Result = "S001";
                }

                var id = user?.UserId;

                if(id != null)
                {
                    var existingPhoto = await context.UserPhotos.FirstOrDefaultAsync(p => p.UserId == id && p.CompanyId == userDetailsModel.CompanyId);

                    if (userDetailsModel.PhotoPath != null && userDetailsModel.PhotoPath.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(userDetailsModel.PhotoPath.FileName);
                        string uploadDir = Path.Combine(webRootPath, "uploads");
                        Directory.CreateDirectory(uploadDir);

                        string filePath = Path.Combine(uploadDir, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await userDetailsModel.PhotoPath.CopyToAsync(stream);
                        }

                        // Optional: Delete old file
                        if (existingPhoto != null && existingPhoto.PhotoPath != null)
                        {
                            string oldPath = Path.Combine(webRootPath, existingPhoto.PhotoPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                            if (System.IO.File.Exists(oldPath))
                            {
                                System.IO.File.Delete(oldPath);
                            }

                            // Update existing photo record
                            existingPhoto.PhotoPath = "/uploads/" + fileName;
                            existingPhoto.ModifiedDate = DateTime.Now;
                            existingPhoto.ModifiedBy = userDetailsModel.ModifiedBy;

                            context.UserPhotos.Update(existingPhoto);
                        }
                        else
                        {
                            // Insert new photo record
                            var newPhoto = new UserPhoto
                            {
                                UserId = id,
                                PhotoPath = "/uploads/" + fileName,
                                CreatedDate = DateTime.Now,
                                CreatedBy = userDetailsModel.CreatedBy,
                                CompanyId = userDetailsModel.CompanyId,
                                IsDeleted = false
                            };
                            context.UserPhotos.Add(newPhoto);
                        }
                    }
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                logger.LogDebug("UserRepository > UserCreation() Completed {model}", userDetailsModel);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex, "Error in UserRepository > UserCreation() {model}", userDetailsModel);
                throw;
            }

            return Result;
        }


        public async Task<string> UserCreationUpdation1(UserDetailsModel userDetailsModel)
        {
            string Result = "";

            try
            {
                logger.LogDebug("UserRepository > UserCreation() started {model}", userDetailsModel);

                var duplicateUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == userDetailsModel.UserName && u.UserId != userDetailsModel.UserId);

                if (duplicateUser != null)
                {
                   return Result = "Exist";
                }

                if(context.Users.Any(u => u.UserId == userDetailsModel.UserId))
                {
                    var user = context.Users.FirstOrDefault(u => u.UserName == userDetailsModel.UserName && u.UserId == userDetailsModel.UserId && u.CompanyId == userDetailsModel.CompanyId);

                    if(user != null)
                    {
                        user.FullName = userDetailsModel.FullName;
                        user.Address = userDetailsModel.Address;
                        user.Email = userDetailsModel.Email;
                        user.Phone = userDetailsModel.Phone;
                        user.Gender = userDetailsModel.Gender;
                        user.DateOfBirth = userDetailsModel.DateOfBirth;
                        user.Password = userDetailsModel.Password;
                        user.RoleId = userDetailsModel.RoleId;
                        user.ModifiedBy = userDetailsModel.UserId;
                        // user.ModifiedDate = DateTime.Now;
                        context.Users.Update(user);

                        Result = "U001";

                    }
                }
                else
                {
                    User user = new User();
                    user.UserName = userDetailsModel.UserName;
                    user.Password = userDetailsModel.Password;
                    user.FullName = userDetailsModel.FullName;
                    user.Address = userDetailsModel.Address;
                    user.Email = userDetailsModel.Email;
                    user.Phone = userDetailsModel.Phone;
                    user.Gender = userDetailsModel.Gender;
                    user.DateOfBirth = userDetailsModel.DateOfBirth;
                    user.RoleId = userDetailsModel.RoleId;
                    user.IsActive = true;
                    user.CreatedBy = userDetailsModel.UserId;
                    user.CreatedDate = DateTime.Now;
                    user.CompanyId = userDetailsModel.CompanyId;
                    user.IsDeleted = false;

                    context.Users.Add(user);

                    Result = "S001";
                }

                context.SaveChanges();

                logger.LogDebug("UserRepository > UserCreation() Completed {model}", userDetailsModel);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in UserRepository > UserCreation() {model}", userDetailsModel);
                throw;
            }

            return Result;
        }

        public async Task<List<UserDetails>> GetUserDetails(CommonModelClass commonModelClass)
        {
            List<UserDetails> ltUserDetails = new List<UserDetails>();
            try
            {
                logger.LogDebug("UserRepository > GetUserDetails() started {model}", commonModelClass);

                ltUserDetails = await (from u in context.Users
                                 join m in context.Memberships on u.UserId equals m.UserId
                                 join p in context.UserPhotos on u.UserId equals p.UserId
                                 where u.CompanyId == commonModelClass.CompanyId && !m.IsDeleted && !u.IsDeleted && !p.IsDeleted && u.IsActive
                                 select (new UserDetails
                                 {
                                     UserName = u.FullName,
                                     UserId = u.UserId,
                                     gender = u.Gender,
                                     PhoneNo = u.Phone,
                                     PhotoPath = p.PhotoPath,
                                     MemberShip = m.StartDate + "" + m.EndDate

                                 })).ToListAsync();

                logger.LogDebug("UserRepository > GetUserDetails() Completed {model}", commonModelClass);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in UserRepository > GetUserDetails() {model}", commonModelClass);
                throw;
            }

            return ltUserDetails;
        }

        public async Task<UserProfile> GetUserProfile(CommonModelClass commonModelClass)
        {
            UserProfile userProfile = new UserProfile();
            List<Dietplans> ltdietplans = new List<Dietplans>();

            try
            {
                logger.LogDebug("UserRepository > GetUserProfile() started {model}", commonModelClass);

                userProfile = await (from u in context.Users
                                      join m in context.Memberships on u.UserId equals m.UserId
                                      join r in context.Ratings on u.UserId equals r.UserId
                                      join t in context.Trainers on u.UserId equals t.TrainerId
                                      join gt in context.GymTypes on m.Type equals gt.TypeId
                                      join p in context.UserPhotos on u.UserId equals p.UserId
                                      where u.UserId == commonModelClass.UserId && u.CompanyId == commonModelClass.CompanyId && !m.IsDeleted && !u.IsDeleted && !p.IsDeleted && u.IsActive
                                      select (new UserProfile
                                      {
                                          UserName = u.FullName,
                                          UserId = u.UserId,
                                          gender = u.Gender,
                                          PhoneNo = u.Phone,
                                          PhotoPath = p.PhotoPath,
                                          StartDate = m.StartDate,
                                          EndDate = m.EndDate,
                                          Review = r.Review,
                                          Star = r.Stars,
                                          ModifiedDate = r.ModifiedDate,
                                          Trainer = t.FullName,
                                          Speciality = gt.TypeName,

                                      })).FirstOrDefaultAsync();


               var dietplans = await context.Dietplans.Where(dp => (dp.UserId == commonModelClass.UserId || dp.UserId == commonModelClass.CompanyId) && dp.CompanyId == commonModelClass.CompanyId && !dp.IsDeleted).ToListAsync();

                foreach(var item in dietplans)
                {
                    Dietplans dietplans1 = new Dietplans();

                    dietplans1.FileName = item.FileName;
                    dietplans1.FilePath = item.FilePath;
                    ltdietplans.Add(dietplans1);
                }

                userProfile.dietplans = ltdietplans;


                logger.LogDebug("UserRepository > GetUserProfile() Completed {model}", commonModelClass);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in GetUserProfile > GetUserDetails() {model}", commonModelClass);
                throw;
            }

            return userProfile;

        }

        public static class InputSanitizer
        {
            public static string Sanitize(string input)
            {
                return string.IsNullOrWhiteSpace(input)
                    ? input
                    : System.Net.WebUtility.HtmlEncode(input.Trim());
            }
        }

    }
}
