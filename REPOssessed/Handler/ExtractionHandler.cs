using Photon.Pun;
using REPOssessed.Util;
using System.Collections.Generic;

namespace REPOssessed.Handler
{
    public class ExtractionPointHandler
    {
        public ExtractionPoint extractionPoint;
        public PhotonView photonView;

        public ExtractionPointHandler(ExtractionPoint extractionPoint)
        {
            this.extractionPoint = extractionPoint;
            this.photonView = extractionPoint.GetComponent<PhotonView>();
        }

        public bool InStartRoom() => extractionPoint.Reflect()?.GetValue<bool>("inStartRoom") ?? false;
        public bool IsShop() => extractionPoint.Reflect()?.GetValue<bool>("isShop") ?? false;

        public ExtractionPoint.State GetCurrentState() => extractionPoint.Reflect()?.GetValue<ExtractionPoint.State>("currentState") ?? ExtractionPoint.State.None;

        public bool IsCompleted() => GetCurrentState() == ExtractionPoint.State.Complete || GetCurrentState() == ExtractionPoint.State.Surplus || GetCurrentState() == ExtractionPoint.State.TaxReturn;

        public void ThiefPunishment()
        {
            if (!GameUtil.IsMasterClient() || !IsShop()) return;
            Patches.forceThiefPunishment = true;
            extractionPoint?.Reflect()?.Invoke("ThiefPunishment");
        }
        
        public void CompleteExtraction()
        {
            if (!GameUtil.IsMasterClient() || IsShop() || IsCompleted()) return;
            if (SemiFunc.IsMultiplayer()) photonView?.RPC("StateSetRPC", RpcTarget.All, ExtractionPoint.State.Complete);
            else extractionPoint.StateSetRPC(ExtractionPoint.State.Complete);
        }
    }

    public static class ExtractionPointHandlerExtensions
    {
        public static Dictionary<int, ExtractionPointHandler> ExtractionPointHandlers = new Dictionary<int, ExtractionPointHandler>();

        public static ExtractionPointHandler? Handle(this ExtractionPoint extractionPoint)
        {
            if (extractionPoint == null) return null;
            int id = extractionPoint.GetInstanceID();
            if (!ExtractionPointHandlers.TryGetValue(id, out ExtractionPointHandler extractionPointHandler))
            {
                extractionPointHandler = new ExtractionPointHandler(extractionPoint);
                ExtractionPointHandlers[id] = extractionPointHandler;
            }
            return extractionPointHandler;
        }
    }
}
