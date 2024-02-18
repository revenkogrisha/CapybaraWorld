using Zenject;
using Core.Editor.Debugger;
using Core.Mediation;

namespace Core.Common.ThirdParty
{

    public class ThirdPartyInitializer
    {
        private readonly IMediationService _mediationService;

        [Inject]
        public ThirdPartyInitializer(IMediationService mediationService) => 
                _mediationService = mediationService;

        public void InitializeAll()
        {
            _mediationService.Initialize();

            RDebug.Info($"{nameof(ThirdPartyInitializer)}: Initialization complete!");
        }
    }
}