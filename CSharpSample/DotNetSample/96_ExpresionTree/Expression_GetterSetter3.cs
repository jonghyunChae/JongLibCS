using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace DotNetSample._96_ExpresionTree;

static class Expression_GetterSetter3
{
    public class CharacterStatsInfo
    {
        public byte ByteValue { get; set; }
        public short ShortValue { get; set; }
        public int IntValue { get; set; }
        public long LongValue { get; set; }
    }

    public enum ValueKind
    {
        ByteValue,
        ShortValue,
        IntValue,
        LongValue
    }

    private static Dictionary<(ValueKind kind, Type statType), Func<CharacterStatsInfo, object>> getterDic { get; } = new();

    static dynamic GetValue(this CharacterStatsInfo statInfo, ValueKind kind)
    {
        if (getterDic.TryGetValue((kind, statInfo.GetType()), out var func))
        {
            return func(statInfo) ?? "";
        }
        return "";
    }

    static Dictionary<(ValueKind kind, Type statType), Action<CharacterStatsInfo, object>> setterDic { get; } = new();

    static void SetValue(this CharacterStatsInfo statInfo, ValueKind kind, dynamic value)
    {
        if (setterDic.TryGetValue((kind, statInfo.GetType()), out var func))
        {
            func(statInfo, (object)value);
        }
    }

    static void Main()
    {
        CharacterStatsInfo characterInfo = new();

        BuildGetter(characterInfo);
        BuildSetter(characterInfo);

        foreach (ValueKind valueKind in Enum.GetValues(typeof(ValueKind)))
        {
            Console.WriteLine($"Character {Enum.GetName(valueKind)} : " + characterInfo.GetValue(valueKind));
        }
    }

    public static List<PropertyInfo> GetStatKindProperties(this CharacterStatsInfo characterStatsInfo)
        => characterStatsInfo
            .GetType()
            .GetProperties()
            .Where(x => Enum.IsDefined(typeof(ValueKind), x.Name))
            .ToList();


    public static void BuildGetter(CharacterStatsInfo characterStatsInfo)
    {
        var statProperties = characterStatsInfo.GetStatKindProperties();
        foreach (var property in statProperties)
        {
            ValueKind statKind = (ValueKind)Enum.Parse(typeof(ValueKind), property.Name);
            Console.WriteLine("GetterV2 : " + statKind);

            var inputInfoParamExpr = Expression.Parameter(typeof(CharacterStatsInfo), "input");
            var propertyExpr = Expression.Property(
                Expression.TypeAs(inputInfoParamExpr, property.ReflectedType!),
                property.Name
            );

            var toObjectExpr = Expression.TypeAs(propertyExpr, typeof(object));
            getterDic[(statKind, characterStatsInfo.GetType())] = Expression.Lambda<Func<CharacterStatsInfo, object>>(toObjectExpr, inputInfoParamExpr)
                .Compile();
        }
    }

    public static void BuildSetter(CharacterStatsInfo characterStatsInfo)
    {
        var statProperties = characterStatsInfo.GetStatKindProperties();
        foreach (var property in statProperties)
        {
            ValueKind statKind = (ValueKind)Enum.Parse(typeof(ValueKind), property.Name);
            Console.WriteLine("Setter : " + statKind);

            var inputInfoParamExpr = Expression.Parameter(typeof(CharacterStatsInfo), "inputInfo");
            var propertyExpr = Expression.Property(
                Expression.TypeAs(inputInfoParamExpr, property.ReflectedType!),
                property.Name
            );

            var valueParamExpr = Expression.Parameter(typeof(object), "value");
            var toOriginTypeExpr = Expression.Convert(valueParamExpr, property.PropertyType);
            setterDic[(statKind, characterStatsInfo.GetType())] = Expression.Lambda<Action<CharacterStatsInfo, object>>(Expression.Assign(propertyExpr, toOriginTypeExpr),
                    inputInfoParamExpr, valueParamExpr)
                .Compile();
        }
    }

    // returns property getter
    public static Func<TObject, TProperty> GetPropGetter<TObject, TProperty>(string propertyName)
    {
        ParameterExpression paramExpression = Expression.Parameter(typeof(TObject), "value");

        Expression propertyGetterExpression = Expression.Property(paramExpression, propertyName);

        Func<TObject, TProperty> result =
            Expression.Lambda<Func<TObject, TProperty>>(propertyGetterExpression, paramExpression).Compile();

        return result;
    }

    // returns property setter:
    public static Action<TObject, TProperty> GetPropSetter<TObject, TProperty>(string propertyName)
    {
        ParameterExpression paramExpression = Expression.Parameter(typeof(TObject));

        ParameterExpression paramExpression2 = Expression.Parameter(typeof(TProperty), propertyName);

        MemberExpression propertyGetterExpression = Expression.Property(paramExpression, propertyName);

        Action<TObject, TProperty> result = Expression.Lambda<Action<TObject, TProperty>>
        (
            Expression.Assign(propertyGetterExpression, paramExpression2), paramExpression, paramExpression2
        ).Compile();

        return result;
    }
}