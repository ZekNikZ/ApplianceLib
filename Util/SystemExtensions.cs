using Kitchen;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Reflection;

namespace ApplianceLib.Util
{
    public static class SystemExtensions
    {
        private static FieldInfo AcceptTransfersInfo = ReflectionUtils.GetField<ResolveTransfers>("AcceptTransfers");
        private static FieldInfo SendTransfersInfo = ReflectionUtils.GetField<ResolveTransfers>("SendTransfers");
        public static void RegisterTransfer<T>(this T system) where T : GenericSystemBase
        {
            if (system.World == null || system.World.GetExistingSystem<ResolveTransfers>() == null)
                return;

            ResolveTransfers transferResolve = system.World.GetExistingSystem<ResolveTransfers>();
            if (system is ISendTransfers sender)
            {
                var sendTransfers = SendTransfersInfo.GetValue(transferResolve) as Dictionary<SystemReference, ISendTransfers>;
                sendTransfers.Add(system, sender);
                SendTransfersInfo.SetValue(transferResolve, sendTransfers);
            }
            else if (system is IAcceptTransfers accepter)
            {
                var acceptTransfers = AcceptTransfersInfo.GetValue(transferResolve) as Dictionary<SystemReference, IAcceptTransfers>;
                acceptTransfers.Add(system, accepter);
                AcceptTransfersInfo.SetValue(transferResolve, acceptTransfers);
            }
        }
    }
}
