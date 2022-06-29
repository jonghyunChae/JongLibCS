using System.Linq.Expressions;
using System.Reflection;

namespace DotNetSample._96_ExpresionTree;

static class Expression_GetterSetter2
{
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

    private static Dictionary<(StatKind statKind, Type statType), Func<CharacterStatsInfo, object>> getterDic { get; } = new();

    static dynamic GetValue(this CharacterStatsInfo statInfo, StatKind statKind)
    {
        if (getterDic.TryGetValue((statKind, statInfo.GetType()), out var func))
        {
            return func(statInfo) ?? "";
        }
        return "";
    }

    static Dictionary<(StatKind statKind, Type statType), Action<CharacterStatsInfo, object>> setterDic { get; } = new();

    static void SetValue(this CharacterStatsInfo statInfo, StatKind statKind, dynamic value)
    {
        if (setterDic.TryGetValue((statKind, statInfo.GetType()), out var func))
        {
            func(statInfo, (object)value);
        }
    }

    static void Main()
    {
        PlayerStatsInfo playerInfo = new();
        CharacterStatsInfo characterInfo = new();

        BuildGetter(playerInfo);
        BuildSetter(playerInfo);

        BuildGetter(characterInfo);
        BuildSetter(characterInfo);

        playerInfo.SetValue(StatKind.HP, 100);
        playerInfo.SetValue(StatKind.Name, "zzzz");
        playerInfo.SetValue(StatKind.Damage, 2);

        characterInfo.SetValue(StatKind.HP, -100);
        characterInfo.SetValue(StatKind.Damage, 5000);

        Console.WriteLine();
        Console.WriteLine("Player : " + playerInfo.GetValue(StatKind.HP));
        Console.WriteLine("Player : " + playerInfo.GetValue(StatKind.Damage));
        Console.WriteLine("Player : " + playerInfo.GetValue(StatKind.Name));

        Console.WriteLine("Character : " + characterInfo.GetValue(StatKind.HP));
        Console.WriteLine("Character : " + characterInfo.GetValue(StatKind.Damage));
        Console.WriteLine("Character : " + characterInfo.GetValue(StatKind.Name));
    }

    public static List<PropertyInfo> GetStatKindProperties(this CharacterStatsInfo characterStatsInfo)
        => characterStatsInfo
            .GetType()
            .GetProperties()
            .Where(x => Enum.IsDefined(typeof(StatKind), x.Name))
            .ToList();


    public static void BuildGetter(CharacterStatsInfo characterStatsInfo)
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
            getterDic[(statKind, characterStatsInfo.GetType())] = Expression.Lambda<Func<CharacterStatsInfo, object>>(toObjectExpr, inputInfoParamExpr)
                .Compile();
        }
    }

    public static void BuildSetter(CharacterStatsInfo characterStatsInfo)
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