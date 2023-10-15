using System.Threading.Tasks;
using CodeBase.Services;

namespace CodeBase.UI.Services.Factory
{
  public interface IUIFactory : IService
  {
    public void CreateFinishCongratulations();
    Task CreateUIRoot();
  }
}