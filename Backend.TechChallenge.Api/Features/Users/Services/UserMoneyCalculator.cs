using Backend.TechChallenge.Api.DAL;

using Const = Backend.TechChallenge.Api.Configuration.MoneyCalculatorConstants;

namespace Backend.TechChallenge.Api.Features.Users.Services;

public interface IUserMoneyCalculator
{
    decimal Calculate(decimal money, UserType userType);
}

public class UserMoneyCalculator : IUserMoneyCalculator
{

    public decimal Calculate(decimal money, UserType userType) => (money, userType) switch
    {
        { userType: UserType.Normal, money: > Const.MinimumMoneyTreshold } => money * (1 + Const.NormalUserPercentage),
        { userType: UserType.Normal, money: < Const.MinimumMoneyTreshold and > Const.NormalUserMinimumTreshold } => money * (1 + Const.NormalUserMinimumPercentage),
        { userType: UserType.SuperUser, money: > Const.MinimumMoneyTreshold } => money * (1 + Const.SuperNormalUserPercentage),
        { userType: UserType.Premium, money: > Const.MinimumMoneyTreshold } => money * (1 + Const.PremiumUserPercentage),
        _ => money
    };
}
