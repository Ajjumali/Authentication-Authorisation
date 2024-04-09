using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary.DTOs
{
    public class ServiceResponce
    {
        public record class  GenerenalResponse(bool Flag ,string Message);
        public record class LoginResponse(bool Flag,string Token, string Message);

    }
}
