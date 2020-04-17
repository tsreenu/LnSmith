using DigitalAppraiser.BuinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace DigitalAppraiser.BuinessLogic.Implementation
{
    public class LoginClass : LoginInterface
    {
        private Models.DBModels.DigitalAppraiserDB _Context = new Models.DBModels.DigitalAppraiserDB();
        public Models.ViewModels.LoginModel Login(string MobileNumber, string Password)
        {
            var isValidUser = _Context.AppraiserDetails.Where(x => x.MobileNumber == MobileNumber && x.IsActive == true).FirstOrDefault();
            Models.ViewModels.LoginModel model = new Models.ViewModels.LoginModel();
            model.MobileNumber = MobileNumber;
            model.Password = Password;
            if (isValidUser != null)
            {
                if (isValidUser.MobileNumber == MobileNumber && isValidUser.Password == Password)
                {
                    model.AppraiserId = isValidUser.AppraiserId;
                    model.ErrorMessage = "Valid User";
                    model.UserName = isValidUser.AppraiserName;
                }
                else
                {
                    model.ErrorMessage = "Mobile number and password not matched.";
                }
            }
            else
            {
                model.ErrorMessage = "Mobile number not registered.";
            }
            return model;
        }
        public int SignUpUser(Models.ViewModels.SignUpModel signUpModel)
        {
            try
            {
                var user = _Context.AppraiserDetails.Where(x => x.MobileNumber == signUpModel.MobileNumber).FirstOrDefault();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Models.ViewModels.SignUpModel, Models.DBModels.AppraiserDetail>();
                });
                IMapper mapper = config.CreateMapper();
                var signup = mapper.Map<Models.ViewModels.SignUpModel, Models.DBModels.AppraiserDetail>(signUpModel);

                if (user == null)
                {
                    signup.CreatedBy = signup.AppraiserName;
                    signup.ModifiedBy = signup.AppraiserName;
                    signup.CreatedOn = DateTime.Now;
                    signup.ModifiedOn = DateTime.Now;
                    signup.IsActive = true;
                    _Context.AppraiserDetails.Add(signup);
                    _Context.SaveChanges();

                    for (int i = 0; i < signUpModel.Banks.Count(); i++)
                    {
                        _Context.AppraiserBanks.Add(new Models.DBModels.AppraiserBank
                        {
                            AppriaserId = signup.AppraiserId,
                            BankId = signUpModel.Banks[i].BankId,
                            CreatedBy = signup.AppraiserName,
                            IsActive = true,
                            ModifiedBy = signup.AppraiserName,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        });
                    }
                    _Context.SubscriptionDetails.Add(new Models.DBModels.SubscriptionDetails
                    {
                        AppraiserId = signup.AppraiserId,
                        PlanId = 1,
                        IsActive = true,
                        SubscriptionStartDate = DateTime.Now,
                        SubscriptionEndDate = DateTime.Now.AddDays(30),
                        SubscriptionId=0
                    });

                    _Context.SaveChanges();
                    return 1;
                }
                else
                {
                    user.ModifiedBy = signup.AppraiserName;
                    user.ModifiedOn = DateTime.Now;
                    user.ShopName = signup.ShopName;
                    user.ShopAddress = signup.ShopAddress;
                    user.ShopNumber = signup.ShopNumber;
                    user.CityId = signup.CityId;
                    user.StateId = signup.StateId;
                    user.MobileNumber = signup.MobileNumber;
                    user.AppraiserNumber = signup.AppraiserNumber;
                    user.AppraiserName = signup.AppraiserName;
                    var previousBanks = _Context.AppraiserBanks.Where(x => x.AppriaserId == user.AppraiserId && x.IsActive == true).Select(x => x.BankId.Value).ToList();

                    var newBanks = signUpModel.Banks.Select(x => x.BankId).ToList();

                    var diff1 = previousBanks.Except(newBanks).ToList();
                    var diff2 = newBanks.Except(previousBanks).ToList();
                    var diff = diff1.Concat(diff2).ToList();

                    for (int i = 0; i < diff.Count(); i++)
                    {
                        int bankid = diff[i];
                        var isExist = _Context.AppraiserBanks.Where(x => x.AppriaserId == user.AppraiserId && x.BankId == bankid && x.IsActive == true).FirstOrDefault();
                        if (isExist == null)
                        {
                            _Context.AppraiserBanks.Add(new Models.DBModels.AppraiserBank
                            {
                                AppriaserId = user.AppraiserId,
                                BankId = bankid,
                                CreatedBy = user.AppraiserName,
                                IsActive = true,
                                ModifiedBy = user.AppraiserName,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now
                            });
                            _Context.TodayRates.Add(new Models.DBModels.TodayRate
                            {
                                BankId = bankid,
                                AppraiserId = user.AppraiserId,
                                Rate = 0,
                                CreatedBy = user.AppraiserName,
                                IsActive = true,
                                ModifiedBy = user.AppraiserName,
                                CreatedOn = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                            });
                        }
                        else
                        {
                            int apraiserId = isExist.AppriaserId.Value;
                            var todayRate = _Context.TodayRates.Where(x => x.AppraiserId == apraiserId && x.BankId == bankid && x.IsActive == true).FirstOrDefault();
                            todayRate.IsActive = false;
                          //  _Context.TodayRates.Remove(todayRate);
                            _Context.AppraiserBanks.Remove(isExist);
                        }
                    }
                    var s=_Context.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public List<Models.DBModels.BankMaster> BanksList()
        {

            var list = _Context.BankMasters.Where(x => x.IsActive == true).ToList();
            return list;
        }
        public List<Models.DBModels.State> GetStates()
        {
            return _Context.States.Where(x => x.IsActive == true).ToList();
        }
        public List<Models.DBModels.City> GetCities(int stateId)
        {
            return _Context.Cities.Where(x => x.StateId == stateId && x.IsActive == true).ToList();
        }
        public Models.ViewModels.LoginModel ChangePwd(string MobileNumber, string Password, string NewPwd)
        {
            Models.ViewModels.LoginModel model = new Models.ViewModels.LoginModel();
            var details = _Context.AppraiserDetails.Where(x => x.MobileNumber == MobileNumber).FirstOrDefault();
            if (details == null)
            {
                model.ErrorMessage = "Mobile number not registered";
                return model;
            }
            if (details.MobileNumber == MobileNumber && details.Password == Password)
            {
                details.Password = NewPwd;
                details.ModifiedOn = DateTime.Now;
                _Context.SaveChanges();
                model.ErrorMessage = "Password changed successfully";
            }
            else
            {
                model.ErrorMessage = "Mobile number and password not matched";
            }
            return model;
        }
        public Models.ViewModels.SignUpModel GetAppraiserDetail(int appraiserId)
        {
            Models.ViewModels.SignUpModel signUpModel = new Models.ViewModels.SignUpModel();
            var model = _Context.AppraiserDetails.Where(x => x.AppraiserId == appraiserId && x.IsActive == true).FirstOrDefault();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Models.DBModels.AppraiserDetail, Models.ViewModels.SignUpModel>();
            });
            IMapper mapper = config.CreateMapper();
            signUpModel = mapper.Map<Models.DBModels.AppraiserDetail, Models.ViewModels.SignUpModel>(model);

            return signUpModel;
        }
        public int?[] GetSelectedBanks(int apraiserId)
        {
            var list = _Context.AppraiserBanks.Where(x => x.AppriaserId == apraiserId && x.IsActive == true).Select(x => x.BankId).ToArray();
            return list;
        }
    }
}