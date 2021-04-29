using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

namespace Mobile1BillingReader
{
    public static class ControlExtension
    {
        private delegate void SetPropertyValueHandler<TResult>(Control source, Expression<Func<Control, TResult>> selector, TResult value);

        public static void SetPropertyValue<TResult>(this Control source, Expression<Func<Control, TResult>> selector, TResult value)
        {
            if (source.InvokeRequired)
            {
                var del = new SetPropertyValueHandler<TResult>(SetPropertyValue);
                source.Invoke(del, source, selector, value);
            }
            else
            {
                var propInfo = ((MemberExpression)selector.Body).Member as PropertyInfo;
                propInfo?.SetValue(source, value, null);
            }
        }
    }
}