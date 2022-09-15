using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using ZzzLab.Json;

namespace ZzzLab.Models
{
    public class I18Next : Dictionary<string, string>, ICopyable, ICloneable, IJsonSupport
    {
        [JsonIgnore]
        internal string Code { set; get; }

        [JsonIgnore]
        public string Text
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Code))
                {
                    if (this.Count > 0)
                    {
                        foreach (var item in this)
                        {
                            return item.Value;
                        }
                    }

                    return null;
                }
                else return Get(Code);
            }
        }

        public I18Next(string code, string value)
        {
            Code = code;
            this.Set(code, value);
        }

        public I18Next()
        {
        }

        public string Get(string code)
        {
            if (this.ContainsKey(code)) return this[code];

            return null;
        }

        public void Set(string code, string text, bool isDefault = false)
        {
            if (this.Any() == false) isDefault = true;

            if (this.ContainsKey(code)) this[code] = text;
            else this.Add(code, text);

            if (isDefault) this.Code = code;
        }

        public I18Next Set(I18Next source)
        {
            this.Clear();

            if (source == null) return this;

            if (source != null && source.Any())
            {
                foreach (var item in source)
                {
                    this.Set(item.Key, item.Value);
                }

                this.Code = source.Code;
            }

            return this;
        }

        #region ICopyable

        public virtual I18Next CopyTo(I18Next target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            target.Clear();
            foreach (var item in this)
            {
                target.Set(item.Key, item.Value);
            }

            target.Code = this.Code;

            return target;
        }

        public virtual I18Next CopyFrom(I18Next source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            this.Clear();
            foreach (var item in source)
            {
                this.Set(item.Key, item.Value);
            }

            return this;
        }

        object ICopyable.CopyTo(object target)
            => CopyTo((I18Next)target);

        object ICopyable.CopyFrom(object source)
            => CopyFrom((I18Next)source);

        #endregion ICopyable

        #region ICloneable

        public virtual I18Next Clone()
            => this.CopyTo(new I18Next());

        object ICloneable.Clone()
            => Clone();

        #endregion ICloneable

        #region IJsonSupport

        public virtual string ToJson()
        {
            JsonSerializerSettings setting = new JsonSerializerSettings
            {
                ContractResolver = new NullToEmptyStringResolver(),
                Formatting = Formatting.Indented,
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                NullValueHandling = NullValueHandling.Ignore
            };

            return this.ToJson(setting);
        }

        #endregion IJsonSupport

        #region override

        public virtual string ToString(string code)
            => this.Get(code) ?? string.Empty;

        public override string ToString()
            => this.Text ?? string.Empty;

        #endregion override
    }
}