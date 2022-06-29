using System.Linq.Expressions;

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

class Expression_GetterSetter
{
    private static Dictionary<StatKind, Func<CharacterStatsInfo, object>> getterDic { get; } = new();
    static dynamic GetValue(StatKind statKind, CharacterStatsInfo statInfo) => getterDic[statKind].Invoke(statInfo);

    static Dictionary<StatKind, Action<CharacterStatsInfo, object>> setterDic { get; } = new();
    static void SetValue(StatKind statKind, CharacterStatsInfo statInfo, dynamic value) => setterDic[statKind](statInfo, (object)value);

    static void Main()
    {
        CharacterStatsInfo playerInfo = new();
        playerInfo.HP = 1;
        playerInfo.Damage = 2;
        //playerInfo.Name = "abc";

        BuildGetter(playerInfo.GetType());
        BuildSetter(playerInfo.GetType());


        SetValue(StatKind.HP, playerInfo, 100);
        //SetValue(StatKind.Name, playerInfo, "abc");
        Console.WriteLine(GetValue(StatKind.HP, playerInfo));
        Console.WriteLine(GetValue(StatKind.Damage, playerInfo));
        //Console.WriteLine(GetValue(StatKind.Name, playerInfo));

    }

    public static void BuildGetter(Type statInfoType)
    {
        var statProperties = statInfoType.GetProperties().Where(x => Enum.IsDefined(typeof(StatKind), x.Name));
        foreach (var property in statProperties)
        {
            StatKind statKind = (StatKind)Enum.Parse(typeof(StatKind), property.Name);
            Console.WriteLine(statKind);

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

    public static void BuildSetter(Type statInfoType)
    {
        var statProperties = statInfoType.GetProperties().Where(x => Enum.IsDefined(typeof(StatKind), x.Name));
        foreach (var property in statProperties)
        {
            StatKind statKind = (StatKind)Enum.Parse(typeof(StatKind), property.Name);
            Console.WriteLine(statKind);

            var statInfoParamExpr = Expression.Parameter(statInfoType, "statsInfo");
            var propertyExpr = Expression.Property(
                statInfoParamExpr,
                property.Name
            );

            var valueParamExpr = Expression.Parameter(typeof(object), "value");
            var toOriginTypeExpr = Expression.Convert(valueParamExpr, property.PropertyType);
            setterDic[statKind] = Expression.Lambda<Action<CharacterStatsInfo, object>>(Expression.Assign(propertyExpr, toOriginTypeExpr),
                    statInfoParamExpr, valueParamExpr)
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