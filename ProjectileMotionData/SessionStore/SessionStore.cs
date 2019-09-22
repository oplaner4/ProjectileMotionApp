using ProjectileMotionSource.Func;
using ProjectileMotionSource.WithRezistance.Func;
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


        public SessionStore SaveProjectileMotionWithRezistance(ProjectileMotionWithRezistance motion)
        {
            GetSessionBase()[SessionStoreConstants.PROJECTILEMOTIONWITHREZISTANCE] = motion;
            return this;
        }

        public ProjectileMotionWithRezistance GetSavedProjectileMotionWithRezistance()
        {
            return GetSessionBase()[SessionStoreConstants.PROJECTILEMOTIONWITHREZISTANCE] as ProjectileMotionWithRezistance;
        }

        public ProjectileMotion GetSavedProjectileMotionWithOrWithoutRezistance()
        {
            return IsSavedAnyMotion() ? (IsSavedProjectileMotion() ? GetSavedProjectileMotion() : GetSavedProjectileMotionWithRezistance()) : null;
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


        public bool IsSavedProjectileMotionWithRezistance()
        {
            return GetSavedProjectileMotionWithRezistance() != null;
        }


        public bool IsSavedAnyMotion ()
        {
            return IsSavedProjectileMotionWithRezistance() || IsSavedProjectileMotion();
        }
    }


    class SessionStoreConstants
    {
        public const string PROJECTILEMOTION = "ProjectileMotion";
        public const string PROJECTILEMOTIONWITHREZISTANCE = "ProjectileMotionWithRezistance";
        public const string ACTIONSANDCONTROLLERSVISITED = "ActionsAndControllersVisited";
    }
}