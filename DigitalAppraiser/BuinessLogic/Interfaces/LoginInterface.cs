using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODELS = DigitalAppraiser.Models;

namespace DigitalAppraiser.BuinessLogic.Interfaces
{
    interface LoginInterface
    {
        Models.ViewModels.LoginModel Login(string MobileNumber, string Password);
        int SignUpUser(Models.ViewModels.SignUpModel signUpModel);
        List<Models.DBModels.BankMaster> BanksList();
        List<Models.DBModels.State> GetStates();
        List<Models.DBModels.City> GetCities(int stateId);
        Models.ViewModels.LoginModel ChangePwd(string MobileNumber, string Password, string NewPwd);
        Models.ViewModels.SignUpModel GetAppraiserDetail(int appraiserId);
        int?[] GetSelectedBanks(int apraiserId);
    }
}
