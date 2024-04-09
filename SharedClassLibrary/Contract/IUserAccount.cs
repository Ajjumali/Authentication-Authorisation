using SharedClassLibrary.DTOs;
using SharedClassLibrary.UserDTO;
using static SharedClassLibrary.DTOs.ServiceResponce;

namespace SharedClassLibrary.Contract
{
    public interface IUserAccount
    {
        Task<GenerenalResponse> CreateAccount(UserDTOs UserDTO);
        Task<LoginResponse> LoginAccount(LoginDTO LoginDTO);

    }
}
