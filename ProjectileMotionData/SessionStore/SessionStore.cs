using ProjectileMotionSource.Func;
using ProjectileMotionSource.WithResistance.Func;
using System.Web;
using System.Collections.Generic;

namespace ProjectileMotionData
{
    public class SessionStore
    {
        private HttpSessionStateBase GetSessionBase()
        {
            return ContextBase.Session;
        }

        public SessionStore(HttpContextBase contextBase)
        {
            ContextBase = contextBase;
        }

        HttpContextBase ContextBase { get; set; }

        public SessionStore SaveProjectileMotion(ProjectileMotion motion)
        {
            GetSessionBase()[SessionStoreConstants.PROJECTILEMOTION] = motion;
            return this;
        }

        public ProjectileMotion GetSavedProjectileMotion()
        {
            return GetSessionBase()[SessionStoreConstants.PROJECTILEMOTION] as ProjectileMotion;
        }


        public SessionStore SaveProjectileMotionWithResistance(ProjectileMotionWithResistance motion)
        {
            GetSessionBase()[SessionStoreConstants.PROJECTILEMOTIONWITHRESISTANCE] = motion;
            return this;
        }

        public ProjectileMotionWithResistance GetSavedProjectileMotionWithResistance()
        {
            return GetSessionBase()[SessionStoreConstants.PROJECTILEMOTIONWITHRESISTANCE] as ProjectileMotionWithResistance;
        }

        public ProjectileMotion GetSavedProjectileMotionWithOrWithoutResistance()
        {
            return IsSavedAnyMotion() ? (IsSavedProjectileMotion() ? GetSavedProjectileMotion() : GetSavedProjectileMotionWithResistance()) : null;
        }

        public SessionStore SaveActionVisited(string controllerName, string actionName)
        {
            Dictionary<string, List<string>> existingActionsAndControllersVisited = GetSavedActionsAndControllersVisited();
            if (existingActionsAndControllersVisited.TryGetValue(controllerName, out List<string> existingActionsVisited))
            {
                if (!existingActionsVisited.Contains(actionName))
                {
                    existingActionsVisited.Add(actionName);
                }
            }
            else existingActionsAndControllersVisited.Add(controllerName, new List<string>() { actionName });

            GetSessionBase()[SessionStoreConstants.ACTIONSANDCONTROLLERSVISITED] = existingActionsAndControllersVisited;
            return this;
        }


        public SessionStore SaveControllerVisited(string controllerName)
        {
            Dictionary<string, List<string>> existingActionsAndControllersVisited = GetSavedActionsAndControllersVisited();
            if (!existingActionsAndControllersVisited.TryGetValue(controllerName, out List<string> existingActionsVisited))
            {
                existingActionsAndControllersVisited.Add(controllerName, new List<string>());
            }

            GetSessionBase()[SessionStoreConstants.ACTIONSANDCONTROLLERSVISITED] = existingActionsAndControllersVisited;
            return this;
        }

        private Dictionary<string, List<string>> GetSavedActionsAndControllersVisited()
        {
            return GetSessionBase()[SessionStoreConstants.ACTIONSANDCONTROLLERSVISITED] as Dictionary<string, List<string>> ?? new Dictionary<string, List<string>>();
        }


        public bool WasActionVisited (string controllerName, string actionName) {
            if (GetSavedActionsAndControllersVisited().TryGetValue(controllerName, out List<string> actionsVisited)) {
                return actionsVisited.Contains(actionName);
            }
            return false;
        }


        public bool WasControllerVisited(string controllerName)
        {
            return GetSavedActionsAndControllersVisited().ContainsKey(controllerName);
        }


        public bool IsSavedProjectileMotion()
        {
            return GetSavedProjectileMotion() != null;
        }


        public bool IsSavedProjectileMotionWithResistance()
        {
            return GetSavedProjectileMotionWithResistance() != null;
        }


        public bool IsSavedAnyMotion ()
        {
            return IsSavedProjectileMotionWithResistance() || IsSavedProjectileMotion();
        }
    }


    class SessionStoreConstants
    {
        public const string PROJECTILEMOTION = "ProjectileMotion";
        public const string PROJECTILEMOTIONWITHRESISTANCE = "ProjectileMotionWithResistance";
        public const string ACTIONSANDCONTROLLERSVISITED = "ActionsAndControllersVisited";
    }
}