using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ZzzLab.Json
{
    /// <summary>
    /// Json Resolver. Null값을 string.Empty 로 변경. Namming Rule은 Camel Case를 적용한다.
    /// </summary>
    public class NullToEmptyStringResolver : CamelCasePropertyNamesContractResolver
    {
        /// <summary>
        ///  null을 공백으로 치완한다.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return type.GetProperties()
                    .Select(p =>
                    {
                        var jp = base.CreateProperty(p, memberSerialization);
                        jp.ValueProvider = new NullToEmptyStringValueProvider(p);
                        return jp;
                    }).ToList();
        }

        /// <summary>
        ///
        /// </summary>
        public class NullToEmptyStringValueProvider : IValueProvider
        {
            private readonly PropertyInfo _MemberInfo;

            /// <summary>
            /// IValueProvider 구현
            /// </summary>
            /// <param name="memberInfo"></param>
            public NullToEmptyStringValueProvider(PropertyInfo memberInfo)
            {
                _MemberInfo = memberInfo;
            }

            /// <summary>
            /// IValueProvider 구현
            /// </summary>
            /// <param name="target"></param>
            /// <returns></returns>
            public object GetValue(object target)
            {
                object result = _MemberInfo.GetValue(target, null);
                if (_MemberInfo.PropertyType == typeof(string) && result == null) result = string.Empty;
                return result;
            }

            /// <summary>
            /// IValueProvider 구현
            /// </summary>
            /// <param name="target"></param>
            /// <param name="value"></param>
            public void SetValue(object target, object value)
                => _MemberInfo.SetValue(target, value, null);
        }
    }
}