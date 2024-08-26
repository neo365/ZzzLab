using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace ZzzLab.Net.LDAP
{
    public static class ADExtensions
    {
        public static string GetProperty(this Principal principal, string property)
        {
            if (principal.GetUnderlyingObject() is DirectoryEntry directoryEntry && directoryEntry.Properties.Contains(property))
            {
                return directoryEntry?.Properties[property]?.Value?.ToString();
            }

            return null;
        }
    }
}