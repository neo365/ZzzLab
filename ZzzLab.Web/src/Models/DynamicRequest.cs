using System.Dynamic;

namespace ZzzLab.Web.Models
{
    public class DynamicRequest : Dictionary<string, object>
    {
        public virtual dynamic ToDynamicModel()
        {
            return this.Aggregate(new ExpandoObject() as IDictionary<string, object>,
                                        (a, p) => { a.Add(p); return a; });
        }
    }
}