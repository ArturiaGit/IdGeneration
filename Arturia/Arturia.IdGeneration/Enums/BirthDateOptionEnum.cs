namespace Arturia.IdGeneration.Enums;

/// <summary>
/// 提供用于确定出生日期的选项。
/// </summary>
public enum BirthDateOptionEnum
{
    /// <summary>
    /// 通过一个具体的出生日期来指定。
    /// </summary>
    BySpecificDate = 0,
    
    /// <summary>
    /// 通过指定当前年龄来反推计算出生日期。
    /// </summary>
    ByAge = 1,
    
    /// <summary>
    /// 随机生成一个在有效范围内的出生日期。
    /// </summary>
    Random = 2
}