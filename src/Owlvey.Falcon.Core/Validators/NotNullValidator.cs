using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Owlvey.Falcon.Core.Validators
{
    public static class NotNullValidator
    {
        public static T Validate<T>(T target, Expression<Func<T, string>> member, object value) where T: class {

            LambdaExpression lambda = (LambdaExpression)member;
            MemberExpression memberExpression;

            if (lambda.Body is LambdaExpression){
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else {
                memberExpression = (MemberExpression)lambda.Body;
            }

            var name = ((PropertyInfo)memberExpression.Member).Name;

            if (target == null) {                
                throw new ApplicationException(string.Format(" type {0}, {1} does not found object for {2} ", typeof(T).Name, name, value));
            }
            return target;
        }
    }
}
