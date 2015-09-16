using cordovaBuild.Data.Model;
using cordovaBuild.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Service
{
    public class AccountService
    {
        public async Task<bool> Register(UserProfile userprofile)
        {
            try
            {
                var userProfileRepo = new UserProfileRepository();
                await userProfileRepo.CreateSync(userprofile);

                var userId = userprofile.Id.ToString();
                if (!string.IsNullOrEmpty(userId))
                {
                    var code = userId.Substring(userId.Length - 6, 6);
                    var badge = new Badge
                    {
                        User = userprofile.Username,
                        Code = code,
                        IsAddressVerified = false,
                        IsDriversLicenseVerified = false,
                        IsPhoneVerified = false,
                        IsVehicleVerified = false
                    };

                    var badgeRepo = new BadgeRepository();
                    await badgeRepo.CreateSync(badge);
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
