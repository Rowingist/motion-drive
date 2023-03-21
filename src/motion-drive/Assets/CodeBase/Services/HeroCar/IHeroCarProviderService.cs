using UnityEngine;

namespace CodeBase.Services.HeroCar
{
  public interface IHeroCarProviderService : IService
  {
    GameObject HeroCar { get; set; }
  }
}