using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
  class UIFactory : IUIFactory
  {
    private const string UIRootPath = "UIRoot";
    
    private readonly IAssetProvider _assets;
    private readonly IStaticDataService _staticData;
    
    private Transform _uiRoot;

    public UIFactory(IAssetProvider assets, IStaticDataService staticData)
    {
      _assets = assets;
      _staticData = staticData;
    }
    
    public void CreateFinishCongratulations()
    {
      WindowConfig config = _staticData.ForWindow(WindowId.FinishCongratulation);
      Object.Instantiate(config.Template, _uiRoot);
    }

    public async Task CreateUIRoot()
    {
      GameObject root = await _assets.Instantiate(UIRootPath);
      _uiRoot = root.transform;
    }
  }
}