using System.Linq.Expressions;
using System.Reflection;

namespace DotNetSample._96_ExpresionTree;

public class CharacterStatsInfo
{
    public int HP { get; set; }
    public int Damage { get; set; }

}

public class PlayerStatsInfo : CharacterStatsInfo
{
    public string Name { get; set; }
}

public enum StatKind
{
    HP,
    Damage,
    Name
}

static class Expression_GetterSetter
{
    private static Dictionary<StatKind, Func<CharacterStatsInfo, object>> getterDic { get; } = new();

    static dynamic GetValue(StatKind statKind, CharacterStatsInfo statInfo)
    {
        if (getterDic.TryGetValue(statKind, out var func))
        {
            return func(statInfo) ?? "";
        }
        return "";
    }

    static Dictionary<StatKind, Action<CharacterStatsInfo, object>> setterDic { get; } = new();

    static void SetValue(StatKind statKind, CharacterStatsInfo statInfo, dynamic value)
    {
        if (setterDic.TryGetValue(statKind, out var func))
        {
            func(statInfo, (object)value);
        }
    }

    static void Main()
    {
        PlayerStatsInfo playerInfo = new();
        playerInfo.HP = 1;
        playerInfo.Damage = 2;
        playerInfo.Name = "abc";

        BuildGetterV2(playerInfo);
        BuildSetterV2(playerInfo);


        SetValue(StatKind.HP, playerInfo, 100);
        SetValue(StatKind.Name, playerInfo, "zzzz");
        Console.WriteLine(GetValue(StatKind.HP, playerInfo));
        Console.WriteLine(GetValue(StatKind.Damage, playerInfo));

        var name = GetValue(StatKind.Name, playerInfo);
        Console.WriteLine(GetValue(StatKind.Name, playerInfo));

    }

    public static List<PropertyInfo> GetStatKindProperties(this CharacterStatsInfo characterStatsInfo)
        => characterStatsInfo
            .GetType()
            .GetProperties()
            .Where(x => Enum.IsDefined(typeof(StatKind), x.Name))
            .ToList();

    // 상속 미지원
    public static void BuildGetterV1(CharacterStatsInfo characterStatsInfo)
    {
        var statProperties = characterStatsInfo.GetStatKindProperties();
        foreach (var property in statProperties)
        {
            StatKind statKind = (StatKind)Enum.Parse(typeof(StatKind), property.Name);
            Console.WriteLine("GetterV1 : " + statKind);

            var statInfoParamExpr = Expression.Parameter(typeof(CharacterStatsInfo), "statsInfo");
            var propertyExpr = Expression.Property(
                statInfoParamExpr,
                property.Name
            );

            var toObjectExpr = Expression.TypeAs(propertyExpr, typeof(object));
            getterDic[statKind] = Expression.Lambda<Func<CharacterStatsInfo, object>>(toObjectExpr, statInfoParamExpr)
                .Compile();
        }
    }

    // 상속 지원
    public static void BuildGetterV2(CharacterStatsInfo characterStatsInfo)
    {
        var statProperties = characterStatsInfo.GetStatKindProperties();
        foreach (var property in statProperties)
        {
            StatKind statKind = (StatKind)Enum.Parse(typeof(StatKind), property.Name);
            Console.WriteLine("GetterV2 : " + statKind);

            var inputInfoParamExpr = Expression.Parameter(typeof(CharacterStatsInfo), "input");
            var propertyExpr = Expression.Property(
                Expression.TypeAs(inputInfoParamExpr, property.ReflectedType!),
                property.Name
            );

            var toObjectExpr = Expression.TypeAs(propertyExpr, typeof(object));
            getterDic[statKind] = Expression.Lambda<Func<CharacterStatsInfo, object>>(toObjectExpr, inputInfoParamExpr)
                .Compile();
        }
    }

    // 상속 미지원
    public static void BuildSetterV1(CharacterStatsInfo characterStatsInfo)
    {
        var statProperties = characterStatsInfo.GetStatKindProperties();
        foreach (var property in statProperties)
        {
            StatKind statKind = (StatKind)Enum.Parse(typeof(StatKind), property.Name);
            Console.WriteLine("Setter : " + statKind);

            var inputInfoParamExpr = Expression.Parameter(typeof(CharacterStatsInfo), "inputInfo");
            var propertyExpr = Expression.Property(
                inputInfoParamExpr,
                property.Name
            );

            var valueParamExpr = Expression.Parameter(typeof(object), "value");
            var toOriginTypeExpr = Expression.Convert(valueParamExpr, property.PropertyType);
            setterDic[statKind] = Expression.Lambda<Action<CharacterStatsInfo, object>>(Expression.Assign(propertyExpr, toOriginTypeExpr),
                    inputInfoParamExpr, valueParamExpr)
                .Compile();
        }
    }

    // 상속 지원
    public static void BuildSetterV2(CharacterStatsInfo characterStatsInfo)
    {
        var statProperties = characterStatsInfo.GetStatKindProperties();
        foreach (var property in statProperties)
        {
            StatKind statKind = (StatKind)Enum.Parse(typeof(StatKind), property.Name);
            Console.WriteLine("Setter : " + statKind);

            var inputInfoParamExpr = Expression.Parameter(typeof(CharacterStatsInfo), "inputInfo");
            var propertyExpr = Expression.Property(
                Expression.TypeAs(inputInfoParamExpr, property.ReflectedType!),
                property.Name
            );

            var valueParamExpr = Expression.Parameter(typeof(object), "value");
            var toOriginTypeExpr = Expression.Convert(valueParamExpr, property.PropertyType);
            setterDic[statKind] = Expression.Lambda<Action<CharacterStatsInfo, object>>(Expression.Assign(propertyExpr, toOriginTypeExpr),
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