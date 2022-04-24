using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lupus
{
    public class HistoryMove : IHistoryMove
    {
        public string PlayerId { get; set; }
        public string ObjectId { get; set; }
        public string TypeName { get; set; }
        public string MethodName { get; set; }

        public object[] Parameters { get; set; }
        public string[] ParameterTypeNames { get; set; }

        public HistoryMove(string playerId, string objectId, string typeName, string methodName, object[] parameters, string[] parameterTypeNames)
        {
            PlayerId = playerId;
            ObjectId = objectId;
            TypeName = typeName;
            MethodName = methodName;
            Parameters = parameters;
            ParameterTypeNames = parameterTypeNames;
            var parameterTypes = ParameterTypeNames.Select(n => Type.GetType(n)).ToArray();
            for (var i = 0; i < parameterTypes.Length; i++)
            {
                var type = parameterTypes[i];
                var parameter = parameters[i];
                if (type.IsEnum)
                    this.Parameters[i] = Enum.ToObject(type, parameter);
                else
                    this.Parameters[i] = Convert.ChangeType(parameters[i], type);
            }

            
        }

        public void Execute(Game game)
        {
            var type = Type.GetType(TypeName);
            var parameterTypes = ParameterTypeNames.Select(n => Type.GetType(n)).ToArray();
            var gameObject = game.GetGameObject<object>(PlayerId, ObjectId);

            if (gameObject == null)
                return;

            

            MethodInfo method = type.GetMethod(MethodName, parameterTypes);
            method.Invoke(gameObject, Parameters);
        }
    }
}
