using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Arturia.IdGeneration.Enums;
using Arturia.IdGeneration.Models;

namespace Arturia.IdGeneration.Services;

public class GenerationService : IGenerationService
{
    #region 私有字段
    private static readonly Random Random = new();
    
    private readonly int[] _maleDigits = [1, 3, 5, 7, 9];
    private readonly int[] _femaleDigits = [0, 2, 4, 6, 8];
    
    // 常见姓氏列表
    private readonly string[] _surnames =
    [
        "李", "王", "张", "刘", "陈", "杨", "赵", "黄", "周", "吴",
        "徐", "孙", "胡", "朱", "高", "林", "何", "郭", "马", "罗"
    ];
    // 常见名字用字列表
    private readonly string[] _givenNameChars =
    [
        "伟", "芳", "娜", "敏", "静", "强", "磊", "军", "洋", "艳",
        "勇", "杰", "娟", "涛", "明", "超", "秀", "平", "刚", "丽",
        "华", "春", "雪", "梅", "文", "博", "雅", "瑞", "莹", "琪"
    ];
    
    // 加权因子
    private readonly int[] _weightFactor = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2];
    // 校验码
    private readonly string[] _checkCode = ["1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2"];
    #endregion
    
    public string Generate(GenerationOptions options)
    {
        List<GenerationResult> results = new();
        for (int i = 0; i < options.GenerationCount; i++)
        {
            int genderCode = GetGender(options.GenderOptionEnum);
            string genderString = genderCode % 2 == 0 ? "女" : "男";
            
            string sequenceCode = $"{Random.Next(0, 100):D2}{genderCode}";//顺序位
            string idCard = options.LocationCode + options.BirthDays[i].Replace("/",string.Empty) + sequenceCode;
            CalculateChecksum(ref idCard);

            GenerationResult result = new()
            {
                IdCard = idCard,
                Name = GetName(options.NameGenerationOptionEnum),
                Location = options.Location,
                Gender = genderString,
                BirthDate = options.BirthDays[i],
                Age = DateTime.Now.Year - DateTime.Parse(options.BirthDays[i]).Year
            };
            results.Add(result);
        }

        var serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            TypeInfoResolver = SourceGenerationContext.Default
        };
        string resultJson = JsonSerializer.Serialize(results,serializerOptions);
        return resultJson;
    }

    #region 私有辅助方法
    /// <summary>
    /// 根据指定的性别选项，生成一个代表性别的顺序码（身份证第17位）。
    /// </summary>
    /// <param name="genderOption">性别选项，可以是男性、女性或随机。</param>
    /// <returns>返回一个单位数字符串，奇数代表男性，偶数代表女性，或一个0-9的随机数字。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当提供了无效的性别选项时抛出。</exception>
    private int GetGender(GenderOptionEnum genderOption) => genderOption switch
    {
        GenderOptionEnum.Male => _maleDigits[Random.Next(0, _maleDigits.Length)],
        GenderOptionEnum.Female => _femaleDigits[Random.Next(0, _femaleDigits.Length)],
        GenderOptionEnum.Random => Random.Next(0, 10),
        _ => throw new ArgumentOutOfRangeException(nameof(genderOption), "提供了无效的性别选项。")
    };

    /// <summary>
    /// 根据指定的姓名选项，生成一个姓名。
    /// </summary>
    /// <param name="nameOption">姓名选项，可以是“无”或“随机”。</param>
    /// <returns>生成的姓名字符串，如果选项为“无”，则返回 null。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当提供了无效的姓名选项时抛出。</exception>
    private string GetName(NameGenerationOptionEnum nameOption) => nameOption switch
    {
        NameGenerationOptionEnum.None => string.Empty,
        NameGenerationOptionEnum.Random => GenerateRandomName(),
        _ => throw new ArgumentOutOfRangeException(nameof(nameOption), "提供了无效的姓名选项。")
    };
    
    /// <summary>
    /// 实际执行随机姓名生成的辅助方法。
    /// </summary>
    /// <returns>一个随机的中文姓名。</returns>
    private string GenerateRandomName()
    {
        // 从姓氏列表中随机选取一个姓
        string surname = _surnames[Random.Next(0, _surnames.Length)];

        // 决定名字是一个字还是两个字（这里设置为大约 20% 的几率是单字名）
        int givenNameLength = Random.Next(0, 5) == 0 ? 1 : 2;

        // 使用 StringBuilder 来高效地拼接字符串
        StringBuilder nameBuilder = new StringBuilder(surname);
        for (int i = 0; i < givenNameLength; i++)
        {
            // 从名字用字列表中随机选取一个字并追加
            nameBuilder.Append(_givenNameChars[Random.Next(0, _givenNameChars.Length)]);
        }

        return nameBuilder.ToString();
    }

    private void CalculateChecksum(ref string idCard)
    {
        if (string.IsNullOrEmpty(idCard) || idCard.Length != 17 || !idCard.All(char.IsDigit))
            throw new ArgumentException("输入必须是17位数字字符串。", nameof(idCard));
        
        int sum = 0;
        for (int i = 0; i < idCard.Length; i++)
            sum += int.Parse(idCard[i].ToString()) * _weightFactor[i];
        int remainder = sum % 11;

        idCard += _checkCode[remainder];
    }
    #endregion
}