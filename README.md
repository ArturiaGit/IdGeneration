# IdGeneration
# 身份证号码生成器 - 使用指南

## 🚀 项目简介

这是一个基于 .NET 8.0 和 Avalonia 框架开发的随机身份证号码生成器应用程序。它能够生成符合中国身份证编码规则的数据，支持多种自定义选项，输出格式为 JSON 文件，适用于开发测试、教学演示等合法合规场景。

## ⚙️ 主要功能

### 1. 出生地选择
- 支持省、市、县三级行政区划选择
- 数据库包含最新行政区划代码

### 2. 出生日期选项
| 选项类型 | 输入方式 | 说明 |
|:--------:|:--------:|:----:|
| 指定日期 | 日期选择器 | 精确到日期的选择 |
| 指定年龄 | 年龄范围输入框 | 格式：最小年龄-最大年龄（如18-25） |
| 随机日期 | 自动生成 | 符合身份证有效日期范围（1900年至今） |

### 3. 其他选项
- **性别控制**：男/女/随机
- **姓名生成**：可选不生成或随机中文姓名
- **生成数量**：1-1000条（默认5条）

### 4. 输出功能
- JSON 格式输出
- 一键复制到剪贴板
- 导出 JSON 文件
- 清空结果按钮

## 🛠️ 技术栈

- **框架**: Avalonia UI 11.0
- **平台**: .NET 8.0
- **运行环境**: Windows
- **开源协议**: MIT License

## 📥 安装与使用

### 预编译版本下载
访问 https: //github.com/yourusername/id-card-generator/releases 下载对应平台的安装包

### 从源码构建
```bash
# 克隆仓库
git clone https: //github.com/yourusername/id-card-generator.git

# 进入项目目录
cd id-card-generator

# 恢复依赖
dotnet restore

# 运行应用
dotnet run

# 构建发布版本（Windows）
dotnet publish -c Release -r win-x64
```

## 🧩 项目结构

```
├── Data
│   ├── AreaCodes.json          # 行政区划代码数据库
│   └── Surnames.json           # 姓氏数据集
├── Models
│   ├── GeneratorConfig.cs      # 生成配置模型
│   └── PersonInfo.cs          # 个人信息模型
├── Services
│   ├── IdCardGenerator.cs     # 核心生成逻辑
│   └── NameGenerator.cs       # 姓名生成服务
├── ViewModels                 # MVVM 视图模型
├── Views                      # Avalonia 视图
└── Program.cs                 # 应用入口
```

## 🌐 JSON 输出示例

```json
[
  {
    "idNumber": "110105199003078934",
    "name": "张明",
    "gender": "男",
    "birthdate": "1990-03-07",
    "area": "北京市/市辖区/朝阳区"
  },
  {
    "idNumber": "440305198512126726",
    "name": "李芳",
    "gender": "女",
    "birthdate": "1985-12-12",
    "area": "广东省/深圳市/南山区"
  }
]
```

## ⚠ 免责声明

### 关于数据生成
1. 本工具生成的身份证号码为**符合编码规则的虚拟数据**，基于：
   - 国家标准 GB11643-1999《公民身份号码》
   - 中国行政区划代码数据集
   - MOD11-2校验码算法
2. 生成的号码在格式上符合规范，但与实际存在的身份证号码如有雷同纯属巧合

### 关于界面设计
- 本工具的用户界面设计参考了 [在线工具大全](https://www.lddgo.net/common/idgenerator) 的布局和交互设计（欢迎大家去体验这个网站，功能很齐全）
- 核心生成算法和代码实现为原创开发

### 使用限制
禁止将本工具用于：
- 任何非法身份认证场景
- 侵犯个人隐私的活动
- 欺诈或网络犯罪行为

使用者应确保完全遵守当地法律法规，开发者对生成数据的任何滥用不承担责任。

## 🙏 贡献指南

欢迎提交 PR 或 Issue：
1. 报告数据更新（行政区划变更等）
2. 优化生成算法
3. 改进 UI/UX 设计
4. 增加测试覆盖率

## 📄 开源协议

本项目遵循 LICENSE，允许自由使用、修改和分发代码，但须保留原始许可声明。

```
MIT License
Copyright (c) 2023 [Your Name
]
```
