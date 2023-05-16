using System;
using System.Collections.Generic;
using System.Linq;

namespace ZzzLab.ExceptionEx
{
    public static class ExceptionExtension
    {
        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem,
            Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem)
            where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }

        public static string GetAllMessages(this System.Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException).Select(ex => ex.Message);
            return String.Join(Environment.NewLine, messages);
        }

        public static IEnumerable<ExceptionInfo> GetAllExceptionInfo(this System.Exception exception)
        {
            return from ex in exception.FromHierarchy(ex => ex.InnerException)
                   select new ExceptionInfo { Message = ex.Message, StackTrace = ex.StackTrace, Source = ex.Source };
        }

        public static string GetAllMessages(this IEnumerable<ExceptionInfo> collection)
        {
            var messages = collection.Select(ex => ex.Message);
            return String.Join(Environment.NewLine, messages);
        }
    }
}