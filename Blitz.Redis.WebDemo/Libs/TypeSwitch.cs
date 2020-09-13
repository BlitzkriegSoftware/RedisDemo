using System;

namespace Blitz.Redis.WebDemo.Libs
{
    /// <summary>
    /// Provides for switching on type (handy for error handling)
    /// <para>From: http://stackoverflow.com/questions/11277036/typeswitching-in-c-sharp</para>
    /// </summary>
    public static class TypeSwitch
    {
        /// <summary>
        /// Case Info
        /// </summary>
        public class CaseInfo
        {
            /// <summary>
            /// Is Default Case
            /// </summary>
            public bool IsDefault { get; set; }
            /// <summary>
            /// Type to switch on
            /// </summary>
            public Type Target { get; set; }
            /// <summary>
            /// Thing to do if this case selected
            /// </summary>
            public Action<object> Action { get; set; }
        }

        /// <summary>
        /// Do the action
        /// </summary>
        /// <param name="source">Source of the action</param>
        /// <param name="cases">Cases</param>
        public static void Do(object source, params CaseInfo[] cases)
        {
            if (source == null) return;

            var type = source.GetType();
            foreach (var entry in cases)
            {
                if (entry.IsDefault || entry.Target.IsAssignableFrom(type))
                {
                    entry.Action(source);
                    break;
                }
            }
        }

        /// <summary>
        /// Case Block
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="action">Action</param>
        /// <returns>Case Information</returns>
        public static CaseInfo Case<T>(Action action)
        {
            return new CaseInfo()
            {
                Action = x => action(),
                Target = typeof(T)
            };
        }

        /// <summary>
        /// Case Block (Generic)
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="action">Action</param>
        /// <returns>Case Information</returns>
        public static CaseInfo Case<T>(Action<T> action)
        {
            return new CaseInfo()
            {
                Action = (x) => action((T)x),
                Target = typeof(T)
            };
        }

        /// <summary>
        /// Default Case Block
        /// </summary>
        /// <param name="action">Action</param>
        /// <returns>Case Information</returns>
        public static CaseInfo Default(Action action)
        {
            return new CaseInfo()
            {
                Action = x => action(),
                IsDefault = true
            };
        }
    }

}
