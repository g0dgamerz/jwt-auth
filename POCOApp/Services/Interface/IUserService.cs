using DataLayer.Models;
using DataLayer.ViewModel;

namespace POCOApp.Services.Interface
{
    public interface IUserService
    {
        public List<Department> GetDepartmentList();
        public Task<AuthenticateResponse> Login(AuthenticateRequest ar);
        public Task<string> Register(RegisterRequest rr);


    }
}
